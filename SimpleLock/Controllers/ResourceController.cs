using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleLock.Processors;
using SimpleLock.Services;

namespace SimpleLock.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly ILogger<ResourceController> logger;
        private readonly IResourceProcessor processor;
        private readonly IResourceService<Resource> service;

        public ResourceController(ILogger<ResourceController> logger, IResourceProcessor processor, IResourceService<Resource> service)
        {
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            this.processor = processor ?? throw new System.ArgumentNullException(nameof(processor));
            this.service = service ?? throw new System.ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                var result = await processor.ProcessAsync(name);
                return Ok(result);
            }
            catch
            {
                return Problem();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Resource resource)
        {
            try
            {
                await service.ExecuteAsync(resource);

                return CreatedAtAction(nameof(Post), resource.Name, new { Key = resource.Name, resource.Type });
            }
            catch (Exception)
            {
                return Problem();
            }
        }
    }
}