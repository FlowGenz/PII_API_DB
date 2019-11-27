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
    public class OrderController : ApiController
    {

        private readonly PII_DBContext dbContext;

        public OrderController(PII_DBContext dbContext) : base(dbContext)
        {
            
        }

        [HttpGet]
        public IEnumerable<DressOrder> Get()
        {
            
        }

        [HttpPost]
        public void Post([FromBody] DressOrder order) {

        }
    }
}
