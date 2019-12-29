using System;
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
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;

namespace api.Controllers
{
    [Produces("application/json")]
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class DressController : ApiController
    {

        private readonly PII_DBContext dbContext;
        private UserManager<User> userManager;
        private readonly Mapper mapper;
        public DressController(PII_DBContext dbContext, UserManager<User> userManager) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.userManager = userManager;
            mapper = new Mapper();
        }

        [HttpGet]
        [ProducesResponseType(typeof(DressDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DressDTO>>> Get()
        {

            IEnumerable<DressDTO> dressesDTO = await dbContext.Dress
                .Include(u => u.User)
                .Select(x => mapper.MapDressToDTO(x))
                .ToListAsync();

            if (!dressesDTO.Any())
                return NotFound("No dress found");

            return Ok(dressesDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DressDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DressDTO>> Get([FromRoute] string id)
        {

            //A changer


            IEnumerable<DressDTO> dressesDTO = await dbContext.Dress
                .Include(u => u.User)
                .Select(x => mapper.MapDressToDTO(x))
                .ToListAsync();

            if (!dressesDTO.Any())
                return NotFound("No dress found");

            return Ok(dressesDTO.First());
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] Dress dress) {

            
            if (dress.DateEndAvailable != null && DateTime.Compare((DateTime)dress.DateEndAvailable, dress.DateBeginAvailable) < 0)
                return BadRequest("The date end available can not be early then the date begin available");

            User patnerFound = await userManager.FindByIdAsync(dress.UserId);
            if (patnerFound == null)
                return BadRequest("Partner does not exist");

            Dress dressFound = await dbContext.Dress.FirstOrDefaultAsync(d => d.DressName == dress.DressName);

            if (dressFound != null)
                return BadRequest("dress already exist");

            dress.User = patnerFound;
            patnerFound.Dress.Add(dress);

            try
            {
                await dbContext.Dress.AddAsync(dress);
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                return Conflict("Conflict detected, transation cancel");
            }

            return Ok("Dress added with success");
        }

        [HttpPut]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Put([FromBody] Dress dress)
        {
            Dress dressFound = await dbContext.Dress.FindAsync(dress.Id);

            if (dressFound == null)
                return NotFound("Dress does not exist");

            if (dressFound.DressName != dress.DressName){

                Dress dressNameFound = await dbContext.Dress.FirstOrDefaultAsync(d => d.DressName == dress.DressName);

                if (dressNameFound != null)
                    return BadRequest("dress name already exist");
            }

            try
            {
                dressFound.DressName = dress.DressName;
                dressFound.Description = dress.Description;
                dressFound.Available = dress.Available;
                dressFound.DateBeginAvailable = dress.DateBeginAvailable;
                dressFound.DateEndAvailable = dress.DateEndAvailable;
                dressFound.Price = dress.Price;
                dbContext.Dress.Update(dressFound);
                dbContext.SaveChanges();
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
        public async Task<ActionResult> Delete([FromRoute] string dressId) {

            Dress dressFound = await dbContext.Dress.FindAsync(dressId);

            if (dressFound == null)
                return NotFound("Dress does not exist");

            try
            {
                dbContext.Dress.Remove(dressFound);
                dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                return Conflict("Conflict detected, transation cancel");
            }

            return Ok("Dress deleted with success");
        }
    }
}
