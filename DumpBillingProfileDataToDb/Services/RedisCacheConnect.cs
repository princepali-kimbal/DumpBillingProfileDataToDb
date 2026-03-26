using DumpBillingProfileDataToDb.Entities;
using DumpBillingProfileDataToDb.Interfaces;
using System.Text.Json;

namespace DumpBillingProfileDataToDb.Services;

public class RedisCacheConnect : IRedisCacheConnect
{
    private readonly IRedisCacheDbContext _redisCacheDbContext;
    private readonly EHES_CachingWrapper.CachingDb _redisDB;
    public RedisCacheConnect(IRedisCacheDbContext redisCacheDbContext)
    {
        _redisDB = _redisCacheDbContext.GetCacheDb();
        _redisCacheDbContext = redisCacheDbContext;
    }
    public async Task AddToCacheVEE(RedisPayload redisPayload)
    {
        try
        {
            var values = new List<RedisProfileValues>();
            TimeSpan timeSpan = new TimeSpan(30, 0, 0, 0, 0);

                //var data = JsonSerializer.Serialize(redisPayload.RedisProfileValues);
                byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(redisPayload.RedisProfileValues);
                byte[] compressedData = Snappier.Snappy.CompressToArray(jsonBytes);
                string redisValue = Convert.ToBase64String(compressedData);

                await _redisDB.SetKeyAsync(redisPayload.Key, redisValue, timeSpan);
        }
        catch (Exception ex)
        {

            Console.WriteLine("Redis is down" + ex.Message);
        }
    }
}
