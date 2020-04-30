using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Statistics;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace SiloHost
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
            var endpoint = new IPEndPoint(IPAddress.Loopback, 11111);

            return Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    //Register any external config
                })
                .ConfigureServices((hostContext, services) =>
                {
                    //Register any local dependencies if required
                    services.AddHostedService<Startup>(); //startup class template added here
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    //Register logger
                })
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder
                        .UseLocalhostClustering()
                        .UsePerfCounterEnvironmentStatistics()
                        .ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory()) //This is necessary for auto discovery of all grains referenced by this silo host 
                        .UseDashboard(options =>
                        {
                            //TODO: Add external configuration - default endpoint: http://localhost:8080
                            //Please note, the dashboard registers its services and grains using ConfigureApplicationParts which disables the automatic discovery of grains in Orleans. 
                        })
                        .Configure<ClusterOptions>(opts =>
                        {
                            opts.ClusterId = "dev";
                            opts.ServiceId = "HellowWorldAPIService";
                        })
                        .Configure<EndpointOptions>(opts =>
                        {
                            opts.AdvertisedIPAddress = IPAddress.Loopback;
                        })
                        .ConfigureServices(services =>
                        {
                            //Register all the dependencies used by grains
                            //e.g. an existing library that is to be injected into one of the actors
                        });
                });
        }
    }
}
