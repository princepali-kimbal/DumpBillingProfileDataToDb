using DumpBillingProfileDataToDb.Entities;

namespace DumpBillingProfileDataToDb.Interfaces;

public interface IRedisCacheConnect
{
    Task AddToCacheVEE(RedisPayload redisPayload);
}
