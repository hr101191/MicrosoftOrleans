using GoodbyeGrainInterface;
using Orleans;
using System.Threading.Tasks;

namespace GoodbyeGrain
{
    public class GoodbyeGrain : Grain, IGoodbye
    {
        public async Task<string> SayGoodbye()
        {
            await Task.Yield();
            return "Goodbye!";
        }
    }
}
