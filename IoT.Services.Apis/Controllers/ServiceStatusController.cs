using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IoT.Services.EventBus;
using IoT.Services.Contracts.Eventing;
using IoT.Services.Contracts.Messaging;
using System.Net;
using IoT.Services.Apis.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Features;
using IoT.Services.Api.Channels;

namespace IoT.Services.Apis.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IEventBus eventBus;

        public ValuesController(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        [HttpPost]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        public void Post([FromBody]MessagePayloadModel value)
        {            
           //TODO: new implementation of message.
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
