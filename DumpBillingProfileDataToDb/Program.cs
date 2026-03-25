using DumpBillingProfileDataToDb.DBContext;
using DumpBillingProfileDataToDb.Interfaces;
using DumpBillingProfileDataToDb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    var configuration = context.Configuration;
    var connectionString = configuration.GetConnectionString("Postgres");

    services.AddDbContextFactory<PostGresqlDBContext>(options =>
        options.UseNpgsql(connectionString));

    // ✅ Register repositories
    services.AddScoped<INamePlateRepository, NamePlateRepository>();
    services.AddScoped<IDataBaseHelper, DatebaseHelper>();
    services.AddScoped<IBillingMappingRepository, BillingMappingRepository>();

    services.AddSingleton<IServiceLocator, ServiceLocator>();
});

var host = builder.Build();

// 🔽 CALL YOUR MAIN METHOD
using (var scope = host.Services.CreateScope())
{
    var repo = scope.ServiceProvider.GetRequiredService<IBillingMappingRepository>();

    await repo.DumpBillingProfileDataToRedis();
}

await host.RunAsync();