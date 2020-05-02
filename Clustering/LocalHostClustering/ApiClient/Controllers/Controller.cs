using System;
using System.Threading.Tasks;
using GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace ApiClient.Controllers
{
    [Route("api")]
    [ApiController]    
    public class Controller : ControllerBase
    {
        private readonly IClusterClient clusterClient;
        private readonly IHelloWorld helloWorldGrain;
        public Controller(IClusterClient clusterClient)
        {
            this.clusterClient = clusterClient;
            //Initialize the grains that the controller instance will use
            helloWorldGrain = this.clusterClient.GetGrain<IHelloWorld>(new Guid());
        }

        [Route("hello")]
        [HttpGet]
        public async Task<string> Hello()
        {
            return await helloWorldGrain.SayHello();
        }
    }
}
