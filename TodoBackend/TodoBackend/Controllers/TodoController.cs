using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft;

namespace TodoBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;
        private readonly IDbConnector _connector;

        public TodoController(ILogger<TodoController> logger, IDbConnector connector)
        {
            _logger = logger;
            _connector = connector;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.Ok(_connector.All().Select(x=> Newtonsoft.Json.JsonConvert.SerializeObject(x)).ToArray());
        }

        [HttpPost]
        public bool Post([FromBody] TodoBuildData data)
        {
            return _connector.Insert(data.Title, data.Content);
        }

        [Route("finish/{id:Guid}")]
        public bool Finish(string id)
        {
            return _connector.finish(id);
        }

        [Route("delete/{id:Guid}")]
        public bool Delete(string id)
        {
            return _connector.Delete(id);
        }

        [Route("delete/all_finished")]
        public int DeleteAll()
        {
            return _connector.DeleteAllDone();
        }
    }
}
