using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using POCRabbitMQ;


namespace RabbitMQWEBApplication.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IConnectionsManager _connectionsManager;
        private readonly IOptions<RabbitMQOptions> _rabbitMQOptions;

        private string Exchanges { get; set; }

        private string routingKey { get; set; }

        public ValuesController(IConfiguration config, IConnectionsManager connectionsManager, IOptions<RabbitMQOptions> RabbitMQOptions)
        {
            _config = config;
            _connectionsManager = connectionsManager;
            _rabbitMQOptions = RabbitMQOptions;
            Exchanges = _rabbitMQOptions.Value.Exchanges["MainExchange"].Name;
            routingKey = _rabbitMQOptions.Value.Exchanges["MainExchange"].Queues["DispatchQueue"].Name;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var channel = _connectionsManager.SendTextAsync("Hello World", Exchanges, routingKey);
            return Ok();
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
