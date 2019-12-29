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
using Microsoft.AspNetCore.Identity;

namespace api.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ApiController
    {

        private readonly PII_DBContext dbContext;
        private readonly Mapper mapper;
        private readonly UserManager<User> userManager;

        public LoginController(PII_DBContext dbContext, UserManager<User> userManager) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.userManager = userManager;
            mapper = new Mapper();
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] LoginDTO userLogin)
        {
            
            User userFound = await userManager.FindByNameAsync(userLogin.Username);

            // renvoyer les string dans les bad request séparer ou ensemble ?
            if (userFound != null)
                return BadRequest("Username or password incorrect");

            bool isPasswordValid = await userManager.CheckPasswordAsync(userFound, userLogin.Password);

            if (!isPasswordValid)
                return BadRequest("Username or password incorrect");

            return Ok("Login with success");

            //Pour test rapide
            /*if (userLogin.Username == "FlowGenZ" && userLogin.Password == "77naruto77")
                return Ok("Login with success");
            return BadRequest("Username or password incorrect");*/
        }
    }
}
