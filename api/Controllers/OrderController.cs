using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public OrderController(PII_DBContext dbContext) : base(dbContext)
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
        public ActionResult<IEnumerable<DressOrder>> Get()
        {
            IEnumerable<DressOrder> dressOrders = dbContext.DressOrder.ToList();

            if (dressOrders.Any()) {
                List<DressOrderDTO> dressesOrdersDTO = new List<DressOrderDTO>();
                foreach (DressOrder dressOrder in dressOrders) {
                    DressOrderDTO dto = Mapper.MapOrderToDTO(dressOrder);
                    dressesOrdersDTO.Add(dto);
                }
                return Ok(dressesOrdersDTO);
            }
            return NotFound("No order found");
        }

        /// <summary>
        /// .!--.!--
        /// </summary>
        /// <param name="dress"></param>
        /// <response code="201">.!--.!--</response>
        /// <response code="400">.!--.!--</response> 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public void Post([FromBody] DressOrder order) {

        }
    }
}
