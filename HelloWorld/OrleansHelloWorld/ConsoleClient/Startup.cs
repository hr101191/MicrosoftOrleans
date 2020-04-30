using GrainInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleClient
{
    public class Startup : IHostedService, IDisposable
    {
        public void Dispose()
        {

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var client = CreateOrleansClient();
            Console.WriteLine("Connected to Orleans cluster successfully...");
            Console.WriteLine("Getting Grain IHelloWorld...");
            var grain = client.GetGrain<IHelloWorld>(new Guid());
            Console.WriteLine("Triggering method SayHello()...");
            string result = await grain.SayHello();
            Console.WriteLine("Result from grain:" + result);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private IClusterClient CreateOrleansClient()
        {
            var clientBuilder = new ClientBuilder()
                .UseLocalhostClustering() //Connects to tcp port 30000 by default
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "ConsoleClient";
                })
                .ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory());

            var client = clientBuilder.Build();

            client.Connect().Wait();            
            return client;
        }
    }
}
