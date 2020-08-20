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

namespace api.Controllers {
    
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("[controller]")]
    public class DressController : ApiController {

        private UserManager<User> userManager;
        public DressController(PII_DBContext dbContext, UserManager<User> userManager) : base(dbContext) {
            this.userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DressDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "PARTNER, ADMIN, CUSTOMER")]
        public async Task<ActionResult<IEnumerable<DressDTO>>> Get() {

            IEnumerable<DressDTO> dressesDTO = await GetPII_DBContext().Dress
                .Include(u => u.User)
                .Select(x => Mapper.MapDressToDTO(x))
                .ToListAsync();

            if (!dressesDTO.Any())
                return NotFound("No dress found");

            return Ok(dressesDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DressDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "PARTNER, ADMIN, CUSTOMER")]
        public async Task<ActionResult<DressDTO>> Get([FromRoute] string id)
        {
            Dress dress = await GetPII_DBContext().Dress
                .Include(u => u.User)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (dress == null)
                return NotFound("No dress found");

            DressDTO dressDTO = Mapper.MapDressToDTO(dress);

            return Ok(dressDTO);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "PARTNER, ADMIN")]
        public async Task<ActionResult> Post([FromBody] DressDTO dressDTO) {

            if (dressDTO.DateEndAvailable != null && DateTime.Compare((DateTime)dressDTO.DateEndAvailable, dressDTO.DateBeginAvailable) < 0)
                return BadRequest("The date end available can not be early then the date begin available");

            User patnerFound = await userManager.FindByIdAsync(dressDTO.PartnerId);
            if (patnerFound == null)
                return BadRequest("Partner does not exist");

            Dress dressFound = await GetPII_DBContext().Dress.FirstOrDefaultAsync(d => d.DressName == dressDTO.DressName);

            if (dressFound != null)
                return BadRequest("dress already exist");

            Dress newDress = Mapper.MapDressDtoToDress(dressDTO, patnerFound);

            await GetPII_DBContext().Dress.AddAsync(newDress);
            await GetPII_DBContext().SaveChangesAsync();

            return Ok("Dress added with success");
        }

        [HttpPut]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "PARTNER, ADMIN")]
        public async Task<ActionResult> Put([FromBody] DressDTO dressDTO)
        {
            Dress dressFound = await GetPII_DBContext().Dress.FindAsync(dressDTO.Id);

            if (dressFound == null)
                return NotFound("Dress does not exist");

            if (dressFound.DressName != dressDTO.DressName){

                Dress dressNameFound = await GetPII_DBContext().Dress.FirstOrDefaultAsync(d => d.DressName == dressDTO.DressName);

                if (dressNameFound != null)
                    return BadRequest("dress name already exist");
            }

            User patnerFound = await userManager.FindByIdAsync(dressDTO.PartnerId);
            if (patnerFound == null)
                return BadRequest("Partner does not exist");

            Dress dressUpdate = Mapper.MapDressDtoToDress(dressDTO, patnerFound);

            try
            {
                GetPII_DBContext().Dress.Update(dressUpdate);
                //dbContext.Entry(dressUpdate).Property("RowVersion").OriginalValue;
                GetPII_DBContext().SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                return Conflict("Conflict detected, transation cancel");
            }

            return Ok("Dress updated with success");
        }

        [HttpDelete("{dressId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "PARTNER, ADMIN")]
        public async Task<ActionResult> Delete([FromRoute] string dressId) {

            Dress dressFound = await GetPII_DBContext().Dress.FindAsync(dressId);

            if (dressFound == null)
                return NotFound("Dress does not exist");

            GetPII_DBContext().Dress.Remove(dressFound);
            GetPII_DBContext().SaveChanges();

            return Ok("Dress deleted with success");
        }
    }
}
