using Microsoft.Extensions.DependencyInjection;

namespace DumpBillingProfileDataToDb.Interfaces;

public interface IServiceLocator : IDisposable
{
    IServiceScope CreateScope();
    T Get<T>();
}
