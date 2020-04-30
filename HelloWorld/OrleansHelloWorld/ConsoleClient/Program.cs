using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ConsoleClient
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
                    //Register any local dependencies if required
                    services.AddHostedService<Startup>(); //startup class template added here
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    //Register logger
                });
        }
    }
}
