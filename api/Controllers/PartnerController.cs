using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using API_DbAccess;
using DTO;
using Microsoft.AspNetCore.Http;

namespace api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class PartnerController : ApiController
    {

        private readonly PII_DBContext dbContext;

        public PartnerController(PII_DBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Get all partners.
        /// </summary>
        /// <response code="201">Returns an IEnumerable of all partners</response>
        /// <response code="400">If the item is null</response>            
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<PartnerDTO>> Get()
        {
            IEnumerable<User> partners = dbContext.User.ToList();

            if (partners.Any()) {
                List<PartnerDTO> partnerDTOs = new List<PartnerDTO>();
                foreach (User partner in partners) {
                    PartnerDTO dto = Mapper.MapPartnerToDTO(partner);
                    partnerDTOs.Add(dto);
                }
                return Ok(partnerDTOs);
            }
            return NotFound("No partner found");
        }
    }
}
