using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API_DbAccess;
using Microsoft.AspNetCore.Http;

namespace api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class DressController : ApiController
    {

        private readonly PII_DBContext dbContext;
        public DressController(PII_DBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// .!--.!--
        /// </summary>
        /// <response code="201">.!--.!--</response>
        /// <response code="400">.!--.!--</response> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Dress>> Get()
        {
            IEnumerable<Dress> dresses = dbContext.Dress.ToList();

            if (dresses.Any()) {
                return Ok(dresses);
            }
            return NotFound("No dress found");
        }

        /// <summary>
        /// .!--.!--.
        /// </summary>
        /// <param name="dress"></param> 
        /// <response code="201">.!--</response>
        /// <response code="400">.!--</response> 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public void Post([FromBody] Dress dress) {

        }
    }
}
