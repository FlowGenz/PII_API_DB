
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_DbAccess;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class PartnerController : DefaultController {
        private readonly UserManager<User> userManager;

        public PartnerController(PII_DBContext dbContext, UserManager<User> userManager) : base(dbContext) {
            this.userManager = userManager;
        }
    
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PartnerDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, CUSTOMER")]
        public async Task<ActionResult<IEnumerable<PartnerDTO>>> Get() {

            IEnumerable<User> users = await userManager.GetUsersInRoleAsync("PARTNER");

            if (!users.Any())
                return NotFound("No partner found");

            IEnumerable<PartnerDTO> customerDTOs = users.Select(x => Mapper.MapPartnerModelToPartnerDTO(x));

            return Ok(customerDTOs);
        }
    }
}