using DumpBillingProfileDataToDb.DBContext;
using DumpBillingProfileDataToDb.Entities;
using DumpBillingProfileDataToDb.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DumpBillingProfileDataToDb.Services;

public class BillingMappingRepository : IBillingMappingRepository
{
    private readonly IDbContextFactory<PostGresqlDBContext> _context;

    public BillingMappingRepository(IDbContextFactory<PostGresqlDBContext> context)
    {
        _context = context;
    }

    public async Task DumpBillingProfileDataToRedis()
    {
        using var db = _context.CreateDbContext();

        Console.WriteLine("🔄 Loading data...");

        // ✅ 1. Get meters
        var meters = await db.NamePlateWithRegister
            .FromSqlRaw("SELECT meternumber, metercategory FROM meter_nameplate")
            .AsNoTracking()
            .ToListAsync();

        // ✅ 2. Get entities (ONLY required table)
        var entities = await db.MeterEntity
            .Where(x => x.ProfileName == "BILLINGPROFILE")
            .AsNoTracking()
            .ToListAsync();

        // ✅ 3. Create lookup (MeterCategory → Entities)
        var entityLookup = entities
            .GroupBy(e => e.MeterCategory)
            .ToDictionary(g => g.Key, g => g.ToList());

        // ✅ 4. Get ProfileId
        var billingProfileId = await db.MeterProfiles
            .Where(x => x.ProfileName == "BILLINGPROFILE")
            .Select(x => x.ProfileId)
            .FirstAsync();

        Console.WriteLine("✅ Data loaded");
        Console.WriteLine("🚀 Processing...");

        int count = 0;

        foreach (var meter in meters)
        {
            // ✅ 5. Get latest billing PER meter (FAST)
            var billing = await db.Set<BillingProfileDto>()
                .FromSqlRaw(@"
                    SELECT meternumber, obisdata, rtcdateat, createdat
                    FROM meter_billingprofile
                    WHERE meternumber = {0}
                    ORDER BY rtcdateat DESC
                    LIMIT 1
                ", meter.MeterNumber)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (billing == null || string.IsNullOrEmpty(billing.Obisdata))
                continue;

            // ✅ 6. Parse OBIS
            var parsed = ParseObisData(billing.Obisdata);

            if (!entityLookup.TryGetValue(meter.MeterCategory, out var meterEntities))
                continue;

            var redisValues = new List<RedisProfileValues>();

            // ✅ 7. Mapping (IMPORTANT LOGIC)
            foreach (var entity in meterEntities)
            {
                if (!parsed.TryGetValue(entity.ObisId, out var value))
                    continue;

                redisValues.Add(new RedisProfileValues
                {
                    EntityCode = entity.EntityCode,
                    ObisCode = entity.ObisCode,
                    Value = value,
                });
            }

            if (redisValues.Count == 0)
                continue;

            // ✅ 8. Redis Key
            var redisKey = $"{meter.MeterNumber}_{meter.MeterCategory}_{billingProfileId}";

            // 🔴 DEBUG PRINT
            Console.WriteLine("=================================");
            Console.WriteLine($"KEY: {redisKey}");
            Console.WriteLine($"VALUES COUNT: {redisValues.Count}");

            foreach (var v in redisValues.Take(3))
            {
                Console.WriteLine($"{v.EntityCode} | {v.ObisCode} | {v.Value}");
            }

            // ✅ 9. Payload
            var redisPayload = new RedisPayload
            {
                Key = redisKey,
                RedisProfileValues = redisValues
            };

            try
            {
                Console.WriteLine(JsonSerializer.Serialize(redisPayload));
                // await _redisCacheConnect.AddToCacheVEE(redisPayload);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Redis error: {ex.Message}");
            }

            count++;

            if (count % 1000 == 0)
                Console.WriteLine($"✅ Processed {count} meters...");
        }

        Console.WriteLine($"🎉 Completed! Total meters: {count}");
    }

    // 🔵 Helper
    private Dictionary<int, string> ParseObisData(string obisData)
    {
        var result = new Dictionary<int, string>();

        if (string.IsNullOrEmpty(obisData))
            return result;

        var items = obisData.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var item in items)
        {
            var clean = item.Trim('(', ')');

            var parts = clean.Split('|');

            if (parts.Length == 2 && int.TryParse(parts[0], out int key))
            {
                result[key] = parts[1];
            }
        }

        return result;
    }
}