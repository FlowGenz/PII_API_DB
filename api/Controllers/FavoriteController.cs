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
using DTO;

namespace api.Controllers
{
    //[Produces("application/json")]
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

        /// <summary>
        /// .!--.!--
        /// </summary>
        /// <response code="200">.!--.!--</response>
        /// <response code="404">.!--.!--</response> 
        [HttpGet]
        [ProducesResponseType(typeof(FavoriteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(String), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<FavoriteDTO>>> Get()
        {
            IEnumerable<FavoriteDTO> favorites = await dbContext.Favorites
            .Select(x => mapper.MapFavoriteToDTO(x))
            .ToListAsync();

            if (favorites.Any()) {
                return Ok(favorites);
            }
            return NotFound("No favorites found");
        }

        /// <summary>
        /// .!--.!--
        /// </summary>
        /// <param name="favorite"></param>
        /// <response code="201">.!--.!--</response>
        /// <response code="400">.!--.!--</response> 
        [HttpPost]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ObjectResult>> Post([FromBody] Favorites favorite) {

            Favorites favoriteFound = await dbContext.Favorites.FirstOrDefaultAsync(f => f.UserId == favorite.UserId && f.DressId == favorite.DressId);

            if (favoriteFound != null)
                return BadRequest("Favorite already exist");

            dbContext.Favorites.Add(favorite);
            dbContext.SaveChanges();
            return Ok("Favorite added with success");
        }

        [HttpDelete("{favoriteID}")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ObjectResult>> Delete([FromBody] int favoriteID) {

            Favorites favoritesFind = await dbContext.Favorites.FirstOrDefaultAsync(f => f.Id == favoriteID);

            if (favoritesFind == null) 
                return NotFound("Favorite not found");

            dbContext.Favorites.Remove(favoritesFind);
            dbContext.SaveChanges();
            return Ok("Favorite deleted with success");
        }
    }
}
