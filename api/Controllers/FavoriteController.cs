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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace api.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class FavoriteController : DefaultController
    {
        private readonly UserManager<User> userManager;

        public FavoriteController(PII_DBContext dbContext, UserManager<User> userManager) : base(dbContext)
        {
            this.userManager = userManager;
        }

        [HttpGet("{username}")]
        [ProducesResponseType(typeof(FavoriteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, CUSTOMER")]
        public async Task<ActionResult<IEnumerable<FavoriteDTO>>> Get([FromRoute] string username)
        {
            User user = await userManager.FindByNameAsync(username);
            if (user == null)
                return BadRequest("Customer must exists");

            IEnumerable<FavoriteDTO> favoritesDress = await GetPII_DBContext().Favorites
                .Include(u => u.Dress)
                .Where(x => x.UserId == user.Id)
                .Select(x => Mapper.MapFavoriteModelToFavoriteDTO(x))
                .ToListAsync();

            if (!favoritesDress.Any())
                return NotFound("No favorites found");

            return Ok(favoritesDress);
        }

        [HttpGet("{username}/{dressId}")]
        [ProducesResponseType(typeof(FavoriteDressDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, CUSTOMER")]
        public async Task<ActionResult<bool>> Get([FromRoute] string username, [FromRoute] string dressId)
        {
            User user = await userManager.FindByNameAsync(username);
            if (user == null)
                return BadRequest("Customer must exists");

            Favorites favoriteFound = GetPII_DBContext().Favorites.FirstOrDefault(d => d.DressId == dressId && d.UserId == user.Id);
            if (favoriteFound == null)
                return NotFound("No favorite found");

            FavoriteDressDTO favoriteDressDTO = new FavoriteDressDTO();

            if (favoriteFound != null)
            {
                favoriteDressDTO.FavoriteId = favoriteFound.Id;
                favoriteDressDTO.IsFavorite = true;
            } 
            else
            {
                favoriteDressDTO.FavoriteId = null;
                favoriteDressDTO.IsFavorite = false;
            } 

            return Ok(favoriteDressDTO);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CUSTOMER")]
        public async Task<ActionResult> Post([FromBody] FavoriteDTO favoriteDTO) {

            Favorites favoriteFound = await GetPII_DBContext().Favorites
                .FirstOrDefaultAsync(f => f.UserId == favoriteDTO.CustomerId && f.DressId == favoriteDTO.DressId);

            if (favoriteFound != null)
                return BadRequest("Favorite already exists");

            User customerExist = await GetPII_DBContext().User.FindAsync(favoriteDTO.CustomerId);
            Dress dressExist = await GetPII_DBContext().Dress.FindAsync(favoriteDTO.DressId);

            if (customerExist == null && dressExist == null)
                return BadRequest("Customer and dress must exist");

            if (customerExist == null)
                return BadRequest("Customer must exists");

            if (dressExist == null)
                return BadRequest("Dress must exists");

            Favorites newFavorite = Mapper.MapFavoriteDtoToFavoriteModel(favoriteDTO, customerExist, dressExist);

            await GetPII_DBContext().Favorites.AddAsync(newFavorite);
            await GetPII_DBContext().SaveChangesAsync();
            return Created("Favorite added with success", null);
        }

        [HttpDelete("{favoriteID}")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CUSTOMER")]
        public async Task<ActionResult> Delete([FromBody] string favoriteID) {

            Favorites favoriteFind = await GetPII_DBContext().Favorites.FirstOrDefaultAsync(f => f.Id == favoriteID);

            if (favoriteFind == null) 
                return NotFound("Favorite not found");

            GetPII_DBContext().Favorites.Remove(favoriteFind);
            await GetPII_DBContext().SaveChangesAsync();
            return Ok("Favorite deleted with success");
        }
    }
}
