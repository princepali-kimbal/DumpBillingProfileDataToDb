using DumpBillingProfileDataToDb.Entities;
using DumpBillingProfileDataToDb.Interfaces;
using EHES_CachingWrapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DumpBillingProfileDataToDb.Services;

public class RedisCacheDbContext : IRedisCacheDbContext
{
    private readonly RedisConnectionParams _redisConnectionParams;
    private readonly ILogger<RedisCacheDbContext> _logger;
    private Lazy<CachingDb> _cachingDb;

    public RedisCacheDbContext(IOptionsMonitor<RedisConnectionParams> redisConnectionParams,
                               ILogger<RedisCacheDbContext> logger)
    {
        _redisConnectionParams = redisConnectionParams.CurrentValue;
        _logger = logger;
        _cachingDb = new Lazy<CachingDb>(CreateNewConnection);
    }
    public CachingDb GetCacheDb()
    {
        return _cachingDb.Value;
    }
    private CachingDb CreateNewConnection()
    {
        try
        {
            _logger.LogInformation($"Creating new Redis connection with endpoints: {string.Join(" ", _redisConnectionParams.Endpoints)}");
            return new CachingDb(_redisConnectionParams.Endpoints, "mymaster", _redisConnectionParams.Password);
            //return new CachingDb(["localhost:6379"], "mymaster", _redisConnectionParams.Password,false);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while creating Redis connection: {ex.Message}");
            throw; // Rethrow exception to ensure the caller is aware of the failure
        }
    }
}
