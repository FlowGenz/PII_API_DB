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
        /// <response code="201">.!--.!--</response>
        /// <response code="400">.!--.!--</response> 
        [HttpGet]
        /*[ProducesResponseType(StatusCodes.Status201Created)] //#warning 200, type
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //#warning 404, type*/
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
        /*[ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]*/
        public void Post([FromBody] Favorites favorite) {

        }
    }
}
