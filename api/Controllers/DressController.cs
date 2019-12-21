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
        /// <response code="201">.!--.!--</response>
        /// <response code="400">.!--.!--</response> 
        [HttpGet]
        [ProducesResponseType(typeof(Dress), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(String), StatusCodes.Status400BadRequest)]
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

            if(!dressesDTO.Any())
                return NotFound("No dress found");

            return Ok(dressesDTO);
            /*IEnumerable<Dress> dresses = dbContext.Dress.Include(u => u.User).ToList();

            if (dresses.Any()) {
                List<DressDTO> dressesDTO = new List<DressDTO>();
                foreach (Dress dress in dresses) {
                    DressDTO dto = Mapper.MapDressToDTO(dress);
                    dressesDTO.Add(dto);
                }
                return Ok(dressesDTO);
            }

            return NotFound("No dress found");*/
        }

        /// <summary>
        /// .!--.!--.
        /// </summary>
        /// <param name="dress"></param> 
        /// <response code="201">.!--</response>
        /// <response code="400">.!--</response> 
        [HttpPost]
        [ProducesResponseType(typeof(Dress), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(String), StatusCodes.Status404NotFound)]
        public void Post([FromBody] Dress dress) {

        }
    }
}
