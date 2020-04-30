using System;
using System.Threading.Tasks;
using GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace ApiClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldController : ControllerBase
    {

        private readonly IClusterClient _client;
        private readonly IHelloWorld _grain;

        public HelloWorldController(IClusterClient client)
        {
            _client = client;
            _grain = _client.GetGrain<IHelloWorld>(new Guid());
        }

        [HttpGet]
        public async Task<string> HelloWorld()
        {
            return await _grain.SayHello();
        }
    }
}
