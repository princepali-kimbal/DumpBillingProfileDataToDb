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
    services.AddScoped<INamePlateRepository, NamePlateRepository>();
    services.AddScoped<IDataBaseHelper, DatebaseHelper>();
    services.AddSingleton<IServiceLocator, ServiceLocator>();
});

var host = builder.Build();
// 🔽 CALL YOUR MAIN FUNCTION HERE
using (var scope = host.Services.CreateScope())
{
    var repo = scope.ServiceProvider.GetRequiredService<INamePlateRepository>();

    var data = await repo.GetNamePlates();

    Console.WriteLine("📦 Data from meter_nameplate:");

    foreach (var item in data)
    {

        Console.WriteLine($"MeterNumber: {item.MeterNumber} | MeterCategory: {item.MeterCategory}");
    }
}

//// 🔽 Test DB Connection
//using (var scope = host.Services.CreateScope())
//{
//    var factory = scope.ServiceProvider
//        .GetRequiredService<IDbContextFactory<PostGresqlDBContext>>();

//    using var db = factory.CreateDbContext();

//    try
//    {
//        //Console.WriteLine("🔄 Checking PostgreSQL connection...");

//        //var canConnect = await db.Database.CanConnectAsync();

//        //if (canConnect)
//        //{
//        //    Console.WriteLine("✅ SUCCESS: Connected to PostgreSQL database!");
//        //}
//        //else
//        //{
//        //    Console.WriteLine("❌ FAILED: Unable to connect to PostgreSQL database.");
//        //}



//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine("❌ ERROR while connecting to database:");
//        Console.WriteLine($"👉 {ex.Message}");
//    }
//}

await host.RunAsync();