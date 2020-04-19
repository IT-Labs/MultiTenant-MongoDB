using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using ItLabs.MultiTenant.Core.MongoDb;

namespace ItLabs.MultiTenant.Api.Controllers
{
    /// <summary>
    /// Controller used to showcase the different tasks per tenant
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IMongoDbContext<Task> _mongoDbContext;

        public TasksController(IMongoDbContext<Task> mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        [HttpGet]
        public IEnumerable<Task> Get()
        {
            var tasks = _mongoDbContext.Get();
            return tasks;
        }

        [HttpPost]
        public string Post([FromForm]Task task)
        {
            _mongoDbContext.Create(task);
            return task.Id.ToString();
        }
    }
}
