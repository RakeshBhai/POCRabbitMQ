using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQWEBApplication.BL;

namespace RabbitMQWEBApplication.Controllers
{
    [Route("api/[controller]")]
    public class ProductServiceController : Controller
    {
        private readonly IRabbitMQBL _rabbitbl;
       public ProductServiceController(IRabbitMQBL rabbitbl)
        {
            _rabbitbl = rabbitbl;
        }
        
        [HttpGet]
        public async Task Get()
        {
            await _rabbitbl.GetAsny();
        }


        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
