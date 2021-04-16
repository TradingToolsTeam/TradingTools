using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using TradingTools.DataLoadService.Entities.Configuration;
using TradingTools.DataLoadService.Services;

namespace TradingTools.DataLoadService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting service");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    IConfigurationSection mainOptions = configuration.GetSection("MainService");
                    IConfigurationSection stockDataOptions = configuration.GetSection("StockDataService");
                    IConfigurationSection influxdbOptions = configuration.GetSection("InfluxdbService");

                    services.Configure<MainOptions>(mainOptions);
                    services.Configure<StockDataOptions>(stockDataOptions);
                    services.Configure<InfluxdbOptions>(influxdbOptions);

                    services.AddSingleton<IMainService, MainService>();
                    services.AddSingleton<IInfluxdbService, InfluxdbService>();
                    services.AddSingleton<IStockDataService, StockDataService>();

                    services.AddHostedService<Worker>();
                });
    }
}
