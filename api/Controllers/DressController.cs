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

namespace api.Controllers
{
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

        /// <summary>
        /// .!--.!--
        /// </summary>
        /// <response code="200">.!--.!--</response>
        /// <response code="400">.!--.!--</response> 
        [HttpGet]
        [ProducesResponseType(typeof(Dress), StatusCodes.Status200OK)]
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

        /// <summary>
        /// .!--.!--.
        /// </summary>
        /// <param name="dress"></param> 
        /// <response code="200">.!--</response>
        /// <response code="400">.!--</response> 
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ObjectResult>> Post([FromBody] Dress dress) {

            if (dress.DateEndAvailable != null && dress.DateEndAvailable.CompareTo(dress.DateBeginAvailable) < 0)
                return BadRequest("The date end available can not be early then the date begin available");

            Dress dressFound = await dbContext.Dress.FirstOrDefaultAsync(d => d.DressName == dress.DressName);

            if (dressFound != null)
                return BadRequest("dress already exist");

            dbContext.Dress.Add(dress);
            return Ok("Dress added with success");
        }

        [HttpPut]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ObjectResult>> Put([FromBody] Dress dress)
        {
            Dress dressFound = await dbContext.Dress.FindAsync(dress.Id);

            if (dressFound == null)
                return NotFound("order does not exist");

            try
            {
                dressFound.DressName = dress.DressName;
                dressFound.Describe = dress.Describe;
                dressFound.Available = dress.Available;
                dressFound.DateBeginAvailable = dress.DateBeginAvailable;
                dressFound.DateEndAvailable = dress.DateEndAvailable;
                dressFound.Price = dress.Price;
                //peut-on les changer ?
                //dressFound.UrlImage = dress.UrlImage;
                //dressFound.UserId == dress.UserId;
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

            dbContext.Dress.Remove(dressFound);
            dbContext.SaveChanges();
            return Ok("Dress deleted with success");
        }
    }
}
