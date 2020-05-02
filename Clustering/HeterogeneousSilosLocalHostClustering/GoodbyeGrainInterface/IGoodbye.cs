using Orleans;
using System.Threading.Tasks;

namespace GoodbyeGrainInterface
{
    public interface IGoodbye : IGrainWithGuidKey
    {
        Task<string> SayGoodbye();
    }
}
