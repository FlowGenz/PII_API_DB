using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_DbAccess;
using DTO;
using Microsoft.AspNetCore.Http;

namespace api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ApiController
    {

        private readonly PII_DBContext dbContext;
        private readonly Mapper mapper;

        public OrderController(PII_DBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            mapper = new Mapper();
        }

        /// <summary>
        /// .!--.!--
        /// </summary>
        /// <response code="201">.!--.!--</response>
        /// <response code="400">.!--.!--</response> 
        [HttpGet]
        [ProducesResponseType(typeof(DressOrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DressOrder>>> Get()
        {
            IEnumerable<DressOrderDTO> dressOrderDTO = await dbContext.DressOrder
                .Select(x => mapper.MapOrderToDTO(x))
                .ToListAsync();

            if (!dressOrderDTO.Any())
                return NotFound("No order found");

            return Ok(dressOrderDTO);
        }

        /// <summary>
        /// .!--.!--
        /// </summary>
        /// <param name="dressOrder"></param>
        /// <response code="200">.!--.!--</response>
        /// <response code="400">.!--.!--</response> 
        [HttpPost]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ObjectResult>> Post([FromBody] DressOrder dressOrder) {

            DressOrder dressOrderFound = await dbContext.DressOrder.FirstOrDefaultAsync(dO => dO.OrderLine == dressOrder.OrderLine && dO.UserId == dressOrder.UserId);

            if (dressOrderFound != null)
                return BadRequest("order already exist");

            dbContext.DressOrder.Add(dressOrder);
            dbContext.SaveChanges();
            return Ok("Dress order added with success");
        }

        [HttpDelete]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ObjectResult>> Delete([FromBody] int dressOrder) {

            DressOrder dressOrderFound = await dbContext.DressOrder.FirstOrDefaultAsync(d => d.Id == dressOrder);

            if (dressOrderFound == null)
                return BadRequest("order does not exist");

            dbContext.DressOrder.Remove(dressOrderFound);
            dbContext.SaveChanges();
            return Ok("Dress order added with success");

        }
    }
}
