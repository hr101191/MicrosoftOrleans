using Microsoft.Extensions.Hosting;
using Orleans.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace AdminSilo
{
    public class Startup : IHostedService
    {
        private ISiloHost siloHost;
        public Startup(ISiloHost siloHost)
        {
            this.siloHost = siloHost;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await siloHost.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await siloHost.StopAsync();
        }
    }
}
