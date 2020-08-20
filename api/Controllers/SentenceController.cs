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
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace api.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class SentenceController : DefaultController
    {
        public SentenceController(PII_DBContext dbContext) : base(dbContext)
        {
        }


        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "PARTNER, ADMIN, CUSTOMER")]
        public async Task<ActionResult<string>> Get()
        {

            IEnumerable<SentencesOfTheDay> sentences = await GetPII_DBContext().SentencesOfTheDay.ToListAsync();
            if (!sentences.Any())
                return BadRequest("Error in the loading of the sentences of the day");

            int random = new Random().Next(0, sentences.Count());


            return Ok(sentences.ElementAt(random).Sentence);
        }
    }
}
