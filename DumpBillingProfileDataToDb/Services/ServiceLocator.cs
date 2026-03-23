using System;
using DumpBillingProfileDataToDb.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DumpBillingProfileDataToDb.Services;

public class ServiceLocator : IServiceLocator
{
    private readonly IServiceProvider _provider;

    public ServiceLocator(IServiceProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public IServiceScope CreateScope()
    {
        return _provider.CreateScope();
    }

    public T Get<T>()
    {
        return _provider.GetRequiredService<T>();
    }

    public void Dispose()
    {
        // Root provider disposal is managed by the Host; no-op here.
    }
}