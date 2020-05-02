using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using Orleans.Statistics;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace SiloHost3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var isConsole = (!Debugger.IsAttached && args.Contains("--console"));
            var isService = (!Debugger.IsAttached && args.Contains("--service"));
            var isSystemd = (!Debugger.IsAttached && args.Contains("--systemd"));

            //Run on console
            if (isConsole)
            {
                CreateHostBuilder(args).UseConsoleLifetime().Build().Run();
            }
            //Run as a service on linux env
            if (isSystemd)
            {
                CreateHostBuilder(args).UseSystemd().Build().Run();
            }
            //Run as windows service
            if (isService)
            {
                CreateHostBuilder(args).UseWindowsService().Build().Run();
            }
            //Run on debugger
            if (!isConsole && !isService && !isSystemd && Debugger.IsAttached)
            {
                CreateHostBuilder(args).Build().Run();
            }
            else
            {
                Console.WriteLine("Invalid setting");
                Environment.Exit(-1);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    //Register any external config
                })
                .ConfigureServices((hostContext, services) =>
                {
                    //Register your local dependencies here
                    services.AddHostedService<Startup>();
                    services.AddSingleton(CreateSiloHost());
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    //Register logger
                    configLogging.AddConsole();
                });
        }

        private static ISiloHost CreateSiloHost()
        {
            var primarySiloEndpoint = new IPEndPoint(IPAddress.Loopback, 11111); //Admin Silo (Primary silo) endpoint

            var siloHostBuilder = new SiloHostBuilder() //Todo: create method at startup.cs to initialize this in startasync method
                .UseLocalhostClustering(11114, 30003, primarySiloEndpoint, "dev", "SiloHostInstance3")
                .UsePerfCounterEnvironmentStatistics()
                /*
                 * ConfigureApplicationParts method is necessary for auto discovery of all grains referenced by this silo host (if Orleans Dashboard is used)
                 * Otherwise, it can be used to customize the Grains hosted by this silo
                 * As a good practice, always add all grains from application base directory
                 */
                .ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory())
                .ConfigureServices(services =>
                {
                    //Register all the dependencies used by grains (e.g. if a grain is required to call a vendor's package)
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    //Register logger
                    configLogging.AddConsole();
                });
            return siloHostBuilder.Build();
        }
    }
}
