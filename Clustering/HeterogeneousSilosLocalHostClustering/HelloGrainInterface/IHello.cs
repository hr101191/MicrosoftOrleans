using Orleans;
using System.Threading.Tasks;

namespace HelloGrainInterface
{
    public interface IHello : IGrainWithGuidKey
    {
        Task<string> SayHello();
    }
}
