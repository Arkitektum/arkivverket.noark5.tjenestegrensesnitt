using System;
using System.Collections.Generic;
using arkitektum.kommit.noark5.api.Controllers;
using arkivverket.noark5tj.models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace arkivverket.noark5tj.webapi.Controllers
{

   
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public class MappeType
        {
            public string systemID { get; set; }
            public string tittel { get; set; }
        }

        [ListWithLinksResult]
        [EnableQuery]
        [HttpGet]
        public ActionResult<IEnumerable<MappeType>> Get()
        {
            var items = new List<MappeType>
            {
                new MappeType() {systemID = "1", tittel = "hello world"},
                new MappeType() {systemID = "2", tittel = "hello you there"},
                new MappeType() {systemID = "3", tittel = "nothing"},
                new MappeType() {systemID = "4", tittel = "test"},
            };

            HttpContext.Items["links"] = new List<LinkType> {new LinkType( "", "http://nrk.no")};

            return Ok(items);
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
