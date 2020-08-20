using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using api.Options;
using System.Reflection;
using System.IO;
using System.Net;
using Microsoft.EntityFrameworkCore;
using API_DbAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using DTO;
using Microsoft.AspNetCore.Identity;
 
namespace api.Controllers
{
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("[controller]")]
    
    public class JwtController : Controller
    {

        private readonly UserManager<User> userManager;
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtController(IOptions<JwtIssuerOptions> jwtOptions, PII_DBContext dbContext, UserManager<User> userManager ) {
            this.userManager = userManager;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost]
        [ProducesResponseType(typeof(JwtDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login([FromBody] LoginDTO model) {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            User customerFound = await userManager.FindByNameAsync(model.Username);
            bool isPasswordValid = await userManager.CheckPasswordAsync(customerFound, model.Password);
            if (customerFound == null || !isPasswordValid)
                return Unauthorized("Username or password invalid");

            var roles = await userManager.GetRolesAsync(customerFound);
            if(roles == null)
                return Unauthorized();

            IEnumerable<Claim> claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, customerFound.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.NameIdentifier, customerFound.UserName),
                new Claim(ClaimTypes.Name, customerFound.UserName),
                new Claim(ClaimTypes.Role, String.Join(';', roles.ToList()))
            };

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials
            );

            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            JwtDTO response = new JwtDTO(
                encodedJwt,
                (int)_jwtOptions.ValidFor.TotalSeconds,
                roles.ToArray()
            );

            return Ok(response);
        }

        private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() -
                            new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                            .TotalSeconds);
    }
}