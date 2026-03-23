using DumpBillingProfileDataToDb.Entities;
using DumpBillingProfileDataToDb.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DumpBillingProfileDataToDb.Services;

public class DatebaseHelper : IDataBaseHelper
{
    private readonly IServiceLocator _serviceScopeFactoryLocator;
    public DatebaseHelper(IServiceLocator serviceScopeFactoryLocator)
    {
        _serviceScopeFactoryLocator = serviceScopeFactoryLocator;
    }
    public async Task<List<NamePlate>> GetNamePlates()
    {

        using var scope = _serviceScopeFactoryLocator.CreateScope();
        var repository =
            scope.ServiceProvider
                .GetService<INamePlateRepository>();
        return await repository.GetNamePlates();
    }
}
