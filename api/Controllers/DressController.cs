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
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet]
        public ActionResult<IEnumerable<Dress>> Get()
        {
            IEnumerable<Dress> dresses = dbContext.Dress.ToList();

            if (dresses.Any()) {
                return Ok(dresses);
            }
            return NotFound("No dress found");
        }

        [HttpPost]
        public void Post([FromBody] Dress dress) {

        }
    }
}
