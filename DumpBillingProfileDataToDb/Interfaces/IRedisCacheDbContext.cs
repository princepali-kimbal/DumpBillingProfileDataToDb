using EHES_CachingWrapper;

namespace DumpBillingProfileDataToDb.Interfaces;

public interface IRedisCacheDbContext
{
    CachingDb GetCacheDb();
}
