using Orleans;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IHelloWorld : IGrainWithGuidKey
    {
        Task<string> SayHello();
    }
}
