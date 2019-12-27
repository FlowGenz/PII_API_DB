using System;
using System.Reflection;
using System.IO;
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

namespace api.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class LoginController
    {

        private readonly PII_DBContext dbContext;
        private readonly Mapper mapper;

        public LoginController(PII_DBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            mapper = new Mapper();
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] LoginDTO userLogin)
        {
            /*User userFound = await dbContext.User.FirstOrDefaultAsync(c => c.Username == userLogin.Username && c.PasswordHash == userLogin.Password);

            if (userFound != null)
                return BadRequest("Username already exist");*/

            if(userLogin.Username == "FlowGenZ" && userLogin.Password == "77naruto77");

            return Ok("Login with success");
        }
    }
}
