using DumpBillingProfileDataToDb.Entities;

namespace DumpBillingProfileDataToDb.Interfaces;

public interface IBillingMappingRepository
{
    //Task<Dictionary<string, List<RedisProfileValues>>> GetBillingMappedData();
    Task DumpBillingProfileDataToRedis();
}