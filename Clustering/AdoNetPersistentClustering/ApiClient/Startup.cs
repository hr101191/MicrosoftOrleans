using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace ApiClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(CreateOrleansClient());
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IClusterClient CreateOrleansClient()
        {
            var clientBuilder = new ClientBuilder()
                /*
                 * Connects to the Admin Silo Port, requests will be routed to the hosting grain
                 * Execptions will be thrown if type of grain is not found
                 */
                .UseAdoNetClustering(opt =>
                {
                    opt.ConnectionString = @"Data Source=DESKTOP-SLLR3II\SQLEXPRESS;Integrated Security=true;Database=OrleansDistributedCache";
                    opt.Invariant = "System.Data.SqlClient"; //For MS SQL Server
                })
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "ApiClient";
                })
                /*
                 * ConfigureApplicationParts method is necessary for auto discovery of all grains referenced by this silo host (if Orleans Dashboard is used)
                 * Otherwise, it can be used to customize the Grains hosted by this silo
                 * As a good practice, always add all grains from application base directory
                 */
                .ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory());

            var client = clientBuilder.Build();

            client.Connect().Wait();
            Console.WriteLine("Connected to orleans cluster successfully...");
            return client;
        }
    }
}
