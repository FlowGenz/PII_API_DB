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

namespace api.Controllers
{
    [Produces("application/json")]
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class DressController : ApiController
    {

        private readonly PII_DBContext dbContext;
        private readonly Mapper mapper;
        public DressController(PII_DBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            mapper = new Mapper();
        }

        [HttpGet]
        [ProducesResponseType(typeof(DressDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DressDTO>>> Get()
        {

            //check userfound;
            /*var userFound = await _userMgr.FindByNameAsync(username);
            if(userFound == null)
                return Unauthorized();*/

            IEnumerable<DressDTO> dressesDTO = await dbContext.Dress
                .Include(u => u.User)
                .Select(x => mapper.MapDressToDTO(x))
                .ToListAsync();

            if (!dressesDTO.Any())
                return NotFound("No dress found");

            return Ok(dressesDTO);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] Dress dress) {

            if (dress.DateEndAvailable != null && dress.DateEndAvailable.CompareTo(dress.DateBeginAvailable) < 0)
                return BadRequest("The date end available can not be early then the date begin available");

            Dress dressFound = await dbContext.Dress.FirstOrDefaultAsync(d => d.DressName == dress.DressName);

            if (dressFound != null)
                return BadRequest("dress already exist");

            try
            {
                dbContext.Dress.Add(dress);
                dbContext.SaveChanges();
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
                return NotFound("order does not exist");

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
        public async Task<ActionResult> Delete([FromBody] int dressId) {

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
