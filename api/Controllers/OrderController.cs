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
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet]
        public ActionResult<IEnumerable<DressOrder>> Get()
        {
            IEnumerable<DressOrder> orders = dbContext.DressOrder.ToList();

            if (orders.Any()) {
                return Ok(orders);
            }
            return NotFound("No order found");
        }

        [HttpPost]
        public void Post([FromBody] DressOrder order) {

        }
    }
}
