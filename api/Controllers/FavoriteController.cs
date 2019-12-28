using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using API_DbAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using DTO;

namespace api.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class FavoriteController : ApiController
    {

        private readonly PII_DBContext dbContext;
        private readonly Mapper mapper;

        public FavoriteController(PII_DBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            mapper = new Mapper();
        }

        [HttpGet("{customerID}")]
        [ProducesResponseType(typeof(DressDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(String), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DressDTO>>> Get([FromBody] string customerID)
        {
            /*IEnumerable<DressDTO> favoritesDress = await dbContext.Favorites.Where(x => x.UserId == customerID)
            .Select(x => mapper.MapFavoriteToDTO(x))
            .ToListAsync();*/

            //if (favoritesDress.Any())
             //   return Ok(favoritesDress);

            return NotFound("No favorites found");
        }

        [HttpPost]
        [ProducesResponseType(typeof(String), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(String), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] string dressId, string customerId) {

            Favorites favoriteFound = await dbContext.Favorites.FirstOrDefaultAsync(f => f.UserId == customerId && f.DressId == dressId);

            if (favoriteFound != null)
                return BadRequest("Favorite already exist");

            User customerExist = await dbContext.User.FindAsync(customerId);
            Dress dressExist = await dbContext.Dress.FindAsync(dressId);

            if (customerExist == null && dressExist == null)
                return BadRequest("Customer and dress do not exist");

            if (customerExist == null)
                return BadRequest("Customer does not exist");

            if (dressExist == null)
                return BadRequest("Dress does not exist");

            Favorites newFavorite = new Favorites();

            newFavorite.DressId = dressId;
            newFavorite.UserId = customerId;

            dbContext.Favorites.Add(newFavorite);
            dbContext.SaveChanges();
            return Created("Favorite added with success", newFavorite.Id);
        }

        [HttpDelete("{favoriteID}")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete([FromBody] string favoriteID) {

            Favorites favoritesFind = await dbContext.Favorites.FirstOrDefaultAsync(f => f.Id == favoriteID);

            if (favoritesFind == null) 
                return NotFound("Favorite not found");

            dbContext.Favorites.Remove(favoritesFind);
            dbContext.SaveChanges();
            return Ok("Favorite deleted with success");
        }
    }
}
