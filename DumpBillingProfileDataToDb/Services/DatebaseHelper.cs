using DumpBillingProfileDataToDb.Entities;
using DumpBillingProfileDataToDb.Interfaces;

namespace DumpBillingProfileDataToDb.Services;

public class DatebaseHelper : IDataBaseHelper
{
    private readonly IServiceLocator _serviceScopeFactoryLocator;
    public DatebaseHelper(IServiceLocator serviceScopeFactoryLocator)
    {
        _serviceScopeFactoryLocator = serviceScopeFactoryLocator;
    }
    public async Task<RedisNamePlate> GetNamePlate(string nodeId, int templateId)
    {

        using var scope = _serviceScopeFactoryLocator.CreateScope();
        var repository =
            scope.ServiceProvider
                .GetService<INamePlateRepository>();
        return await repository.GetNamePlate(nodeId, templateId);
    }
}
