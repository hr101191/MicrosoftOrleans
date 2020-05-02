using GoodbyeGrainInterface;
using HelloGrainInterface;
using Orleans;
using System.Threading.Tasks;

namespace HelloGrain
{
    public class HelloGrain : Grain, IHello
    {
        public async Task<string> SayHello()
        {
            await Task.Yield();
            return "Hello!";
        }

        public async Task<string> SayGoodbyeFromHelloGrain()
        {        
            return await GrainFactory.GetGrain<IGoodbye>(this.GetPrimaryKey()).SayGoodbye();
        }
    }
}
