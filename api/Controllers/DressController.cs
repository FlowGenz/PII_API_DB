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
        public DressController(PII_DBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<DressDTO>> Get()
        {
            IEnumerable<Dress> dresses = dbContext.Dress.Include(p => p.Partners).ThenInclude(u => u.UsernameUserNavigation).ToList();

            if (dresses.Any()) {
                List<DressDTO> dressesDTO = new List<DressDTO>();
                foreach (Dress dress in dresses) {
                    DressDTO dto = Mapper.MapDressToDTO(dress);
                    dressesDTO.Add(dto);
                }
                return Ok(dressesDTO);
            }
            return NotFound("No dress found");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public void Post([FromBody] Dress dress) {

        }
    }
}
