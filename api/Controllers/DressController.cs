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

        private readonly PII_DBContext dbContext;
        public DressController(PII_DBContext dbContext) : base(dbContext)
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
