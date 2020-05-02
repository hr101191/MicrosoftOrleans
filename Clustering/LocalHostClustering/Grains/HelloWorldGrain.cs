using GrainInterfaces;
using Orleans;
using System.Threading.Tasks;

namespace Grains
{
    public class HelloWorldGrain : Grain, IHelloWorld
    {
        public async Task<string> SayHello()
        {
            await Task.Yield();
            return "Hello world";
        }
    }
}
