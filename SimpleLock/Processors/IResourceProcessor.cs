using System;
using System.Threading.Tasks;
using static SimpleLock.Program;

namespace SimpleLock.Processors
{
    public interface IResourceProcessor
    {
        Task<Resource> ProcessAsync(string resourceName);
    }
}