
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
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class PartnerController : ApiController
    {

        private readonly PII_DBContext dbContext;
        private readonly Mapper mapper;

        public PartnerController(PII_DBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            mapper = new Mapper();
        }

        /// <summary>
        /// Get all partners.
        /// </summary>
        /// <response code="200">Returns an IEnumerable of all partners</response>
        /// <response code="400">If the item is null</response>            
        [HttpGet]
        [ProducesResponseType(typeof(PartnerDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PartnerDTO>>> Get()
        {

            IEnumerable<PartnerDTO> partnerDTO = await dbContext.User
                //.Where() si user est un partenair
                .Select(x => mapper.MapPartnerToDTO(x))
                .ToListAsync();

            if (!partnerDTO.Any())
                return NotFound("No partner found");

            return Ok(partnerDTO);
        }
    }
}