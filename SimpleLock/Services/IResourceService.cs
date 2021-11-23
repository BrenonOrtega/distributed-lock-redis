
using System.Threading.Tasks;

namespace SimpleLock.Services
{
    public interface IResourceService<in Tin>
    {
        Task ExecuteAsync(Tin input);
    }
}