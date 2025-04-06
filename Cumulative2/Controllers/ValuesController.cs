using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cumulative2.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> GetValues()
        {
            return new string[] { "data1", "data2" };
        }

        // GET api/values/5
        public string GetById(int id)
        {
            return "data";
        }

        // POST api/values
        public void PostValue([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void PutValue(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void DeleteValue(int id)
        {
        }
    }
}
