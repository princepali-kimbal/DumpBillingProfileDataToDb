using DumpBillingProfileDataToDb.Entities;

namespace DumpBillingProfileDataToDb.Interfaces;

public interface IDataBaseHelper
{
    Task<RedisNamePlate> GetNamePlate(string nodeId, int templateId);
}
