
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

namespace api.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class PartnerController : ApiController
    {

        private readonly PII_DBContext dbContext;
        private readonly Mapper mapper;
        private readonly UserManager<User> userManager;

        public PartnerController(PII_DBContext dbContext, UserManager<User> userManager) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.userManager = userManager;
            mapper = new Mapper();
        }
    
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PartnerDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PartnerDTO>>> Get()
        {

            IEnumerable<User> users = await userManager.GetUsersInRoleAsync("PARTNER");

            if (!users.Any())
                return NotFound("No partener found");

            IEnumerable<PartnerDTO> customerDTOs = users.Select(x => mapper.MapPartnerToDTO(x));

            /*IEnumerable <CustomerDTO> customerDTO = await dbContext.User
                .Select(x => mapper.MapCustomerToDTO(x))
                .ToListAsync();*/

            return Ok(customerDTOs);
        }
    }
}