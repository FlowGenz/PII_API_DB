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
    public class CustomerController : ApiController
    {

        private readonly PII_DBContext dbContext;

        public CustomerController(PII_DBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            IEnumerable<Customer> customers = dbContext.Customer.ToList();

            if (customers.Any()) {
                return Ok(customers);
            }
            return NotFound("No customer found");
        }

        [HttpPost]
        public void Post([FromBody] Customer customer) {

        }
    }
}
