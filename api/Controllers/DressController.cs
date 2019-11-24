using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API_DbAccess;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DressController : ApiController
    {

        public DressController()
        {
            
        }

        [HttpGet]
        public IEnumerable<Dress> Get()
        {
            
        }

        [HttpPost]
        public void Post([FromBody] Dress dress) {

        }
    }
}
