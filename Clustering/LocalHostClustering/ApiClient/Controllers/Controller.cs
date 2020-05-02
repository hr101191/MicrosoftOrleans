﻿using System;
using System.Threading.Tasks;
using GoodbyeGrainInterface;
using HelloGrainInterface;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace ApiClient.Controllers
{
    [Route("api")]
    [ApiController]    
    public class Controller : ControllerBase
    {
        private readonly IClusterClient clusterClient;
        private readonly IHello helloGrain;
        private readonly IGoodbye goodbyeGrain;
        public Controller(IClusterClient clusterClient)
        {
            this.clusterClient = clusterClient;
            //Initialize the grains that the controller instance will use
            helloGrain = this.clusterClient.GetGrain<IHello>(new Guid());
            goodbyeGrain = this.clusterClient.GetGrain<IGoodbye>(new Guid());
        }

        [Route("hello")]
        [HttpGet]
        public async Task<string> Hello()
        {
            return await helloGrain.SayHello();
        }

        [Route("goodbye")]
        [HttpGet]
        public async Task<string> Goodbye()
        {
            return await goodbyeGrain.SayGoodbye();
        }
    }
}
