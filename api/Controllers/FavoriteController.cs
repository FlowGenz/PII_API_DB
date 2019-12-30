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

namespace api.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class FavoriteController : ApiController
    {

        private readonly PII_DBContext dbContext;
        private readonly Mapper mapper;
        private readonly UserManager<User> userManager;

        public FavoriteController(PII_DBContext dbContext, UserManager<User> userManager) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.userManager = userManager;
            mapper = new Mapper();
        }

        [HttpGet("{username}")]
        [ProducesResponseType(typeof(FavoriteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(String), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<FavoriteDTO>>> Get([FromRoute] string username)
        {
            User user = await userManager.FindByNameAsync(username);
            if (user == null)
                return BadRequest("Customer does not exist");

            IEnumerable<FavoriteDTO> favoritesDress = await dbContext.Favorites
                .Include(u => u.Dress)
                .Where(x => x.UserId == user.Id)
                .Select(x => mapper.MapFavoriteToDTO(x))
                .ToListAsync();

            if (!favoritesDress.Any())
                return NotFound("No favorites found");

            /*List<FavoriteDTO> favoriteDTO = new List<FavoriteDTO>();
            FavoriteDTO dto;

            foreach (Favorites favorites in favoritesDress){
                dto = new FavoriteDTO();
                dto.Id = favorites.Id;
                dto.DressName = favorites.Dress.DressName;
                dto.DressPrice = favorites.Dress.Price;
                dto.UrlImage = favorites.Dress.UrlImage;
                dto.Available = favorites.Dress.Available;
                favoriteDTO.Add(dto);
            }*/


            return Ok(favoritesDress);

        }

        [HttpGet("{username}/{dressId}")]
        [ProducesResponseType(typeof(FavoriteDressDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(String), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> Get([FromRoute] string username, [FromRoute] string dressId)
        {
            User user = await userManager.FindByNameAsync(username);
            if (user == null)
                return BadRequest("Customer does not exist");

            Favorites favoriteFound = dbContext.Favorites.FirstOrDefault(d => d.DressId == dressId && d.UserId == user.Id);

            FavoriteDressDTO favoriteDressDTO = new FavoriteDressDTO();
            favoriteDressDTO.IsFavorite = favoriteFound != null;
            if (favoriteDressDTO.IsFavorite)
            {
                favoriteDressDTO.FavoriteId = favoriteFound.Id;
            } else
            {
                favoriteDressDTO.FavoriteId = "";
            } 



            return Ok(favoriteDressDTO);
        }

        [HttpPost]
        [ProducesResponseType(typeof(String), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(String), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] FavoriteDTO favoriteDTO) {

            Favorites favoriteFound = await dbContext.Favorites.FirstOrDefaultAsync(f => f.UserId == favoriteDTO.CustomerId && f.DressId == favoriteDTO.DressId);

            if (favoriteFound != null)
                return BadRequest("Favorite already exist");

            User customerExist = await dbContext.User.FindAsync(favoriteDTO.CustomerId);
            Dress dressExist = await dbContext.Dress.FindAsync(favoriteDTO.DressId);

            if (customerExist == null && dressExist == null)
                return BadRequest("Customer and dress do not exist");

            if (customerExist == null)
                return BadRequest("Customer does not exist");

            if (dressExist == null)
                return BadRequest("Dress does not exist");

            Favorites newFavorite = new Favorites();

            newFavorite.DressId = favoriteDTO.DressId;
            newFavorite.UserId = favoriteDTO.CustomerId;

            dbContext.Favorites.Add(newFavorite);
            dbContext.SaveChanges();
            return Created("Favorite added with success", newFavorite.Id);
        }

        [HttpDelete("{favoriteID}")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete([FromBody] string favoriteID) {

            Favorites favoriteFind = await dbContext.Favorites.FirstOrDefaultAsync(f => f.Id == favoriteID);

            if (favoriteFind == null) 
                return NotFound("Favorite not found");

            dbContext.Favorites.Remove(favoriteFind);
            dbContext.SaveChanges();
            return Ok("Favorite deleted with success");
        }
    }
}
