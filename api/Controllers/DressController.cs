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
using api.DTO;

namespace api.Controllers {
    
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("[controller]")]
    public class DressController : DefaultController {

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
                .Include(d => d.User)
                .Select(d => Mapper.MapDressModelToDressDTO(d))
                .ToListAsync();

            if (!dressesDTO.Any())
                return NotFound("No dress found");

            return Ok(dressesDTO);
        }

        //TODO Héritage

        [HttpGet("{pageIndex}/{pageSize}")]
        [ProducesResponseType(typeof(PaginationDressDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, CUSTOMER")]
        public async Task<ActionResult<PaginationDressDTO>> GetAllDressWithPagination([FromRoute]int pageIndex = 0, [FromRoute] int pageSize = 6)
        {

            IEnumerable<DressDTO> dressesDTO = await GetPII_DBContext().Dress
                .Include(d => d.User)
                .OrderBy(d => d.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(x => Mapper.MapDressModelToDressDTO(x))
                .ToListAsync();

            int count = await GetPII_DBContext().Dress.CountAsync();

            if (count > 0 && dressesDTO.Any())
                return Ok(new PaginationDressDTO(dressesDTO, pageSize, pageIndex, (int)Math.Ceiling(count/(double)pageSize)));
            return NotFound("No dress found");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DressDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "PARTNER, ADMIN, CUSTOMER")]
        public async Task<ActionResult<DressDTO>> Get([FromRoute] string id)
        {
            Dress dress = await GetPII_DBContext().Dress
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dress == null)
                return NotFound("No dress found");

            DressDTO dressDTO = Mapper.MapDressModelToDressDTO(dress);

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

            Dress newDress = Mapper.MapDressDtoToDressModel(dressDTO, patnerFound);

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

            User patnerFound = await userManager.FindByIdAsync(dressDTO.PartnerId);
            if (patnerFound == null)
                return BadRequest("Partner does not exist");

            Dress dressUpdate = Mapper.MapDressDtoToDressModel(dressDTO, patnerFound);

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
            await GetPII_DBContext().SaveChangesAsync();

            return Ok("Dress deleted with success");
        }
    }
}
