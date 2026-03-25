using DumpBillingProfileDataToDb.DBContext;
using DumpBillingProfileDataToDb.Entities;
using DumpBillingProfileDataToDb.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        var meters = await db.NamePlateWithRegister
            .FromSqlRaw("SELECT meternumber, metercategory FROM meter_nameplate")
            .AsNoTracking()
            .ToListAsync();

        var entities = await db.MeterEntity
            .Where(x => x.ProfileName == "BILLINGPROFILE")
            .AsNoTracking()
            .ToListAsync();

        var subTemplates = await db.MeterSubTemplate
            .Where(x => x.ProfileName == "BILLINGPROFILE")
            .Select(x => new
            {
                x.MeterCategory,
                x.ProfileName,
                x.ObisCode,
                x.DataFormat
            })
            .ToListAsync();

        // 🔴 Latest billing
        var billingProfiles = await db.Set<BillingProfileDto>()
            .FromSqlRaw(@"
            SELECT DISTINCT ON (meternumber)
                meternumber,
                obisdata,
                createdat
            FROM meter_billingprofile
            ORDER BY meternumber, createdat DESC
        ")
            .AsNoTracking()
            .ToListAsync();

        var profileLookup = await db.MeterProfiles
            .ToDictionaryAsync(x => x.ProfileName, x => x.ProfileId);

        var billingProfileId = profileLookup["BILLINGPROFILE"];

        Console.WriteLine("✅ Data loaded");

        // 🔹 Lookups
        var entityLookup = entities
            .GroupBy(e => e.MeterCategory)
            .ToDictionary(g => g.Key, g => g.ToList());

        var billingLookup = billingProfiles
            .ToDictionary(x => x.Meternumber, x => x);

        var entityKeyLookup = entities.ToDictionary(
            e => e,
            e => $"{e.MeterCategory}_BILLINGPROFILE_{e.ObisCode}"
        );

        var subTemplateLookup = subTemplates
            .GroupBy(x => new { x.MeterCategory, x.ProfileName, x.ObisCode })
            .ToDictionary(
                g => $"{g.Key.MeterCategory}_{g.Key.ProfileName}_{g.Key.ObisCode}",
                g => g.Select(x => x.DataFormat).Where(x => x != null).Distinct().ToList()
            );

        Console.WriteLine("🚀 Processing & pushing to Redis...");

        int count = 0;

        foreach (var meter in meters)
        {
            if (!billingLookup.TryGetValue(meter.MeterNumber, out var billing))
                continue;

            if (billing == null || string.IsNullOrEmpty(billing.Obisdata))
                continue;

            var parsed = ParseObisData(billing.Obisdata);

            foreach (var kv in parsed)
            {
                Console.WriteLine($"Parsed Key: {kv.Key}, Value: {kv.Value}");
            }

            if (!entityLookup.TryGetValue(meter.MeterCategory, out var meterEntities))
                continue;

            var redisValues = new List<RedisProfileValues>();

            foreach (var entity in meterEntities)
            {
                if (!parsed.TryGetValue(entity.ObisId, out var value))
                    continue;

                var key = entityKeyLookup[entity];

                if (!subTemplateLookup.TryGetValue(key, out var formats))
                    continue;

                foreach (var format in formats)
                {
                    redisValues.Add(new RedisProfileValues
                    {
                        EntityCode = entity.EntityCode,
                        ObisCode = entity.ObisCode,
                        Value = value,
                        DataFormat = format
                    });
                }
            }

            if (redisValues.Count == 0)
                continue;

            var redisKey = $"{meter.MeterNumber}_{meter.MeterCategory}_{billingProfileId}";

            // 🔴 PRINT BEFORE INSERT
            Console.WriteLine("=================================");
            Console.WriteLine($"KEY: {redisKey}");
            Console.WriteLine($"VALUES COUNT: {redisValues.Count}");

            foreach (var v in redisValues.Take(5)) // print only few to avoid spam
            {
                Console.WriteLine($"{v.EntityCode} | {v.ObisCode} | {v.Value} | {v.DataFormat}");
            }

            // 🔴 CREATE PAYLOAD
            var redisPayload = new RedisPayload
            {
                Key = redisKey,
                RedisProfileValues = redisValues
            };

            try
            {
                //await _redisCacheConnect.AddToCacheVEE(redisPayload);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Redis error for {redisKey}: {ex.Message}");
            }

            count++;

            if (count % 1000 == 0)
            {
                Console.WriteLine($"✅ Processed {count} meters...");
            }
        }

        Console.WriteLine($"🎉 Completed! Total meters pushed: {count}");
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