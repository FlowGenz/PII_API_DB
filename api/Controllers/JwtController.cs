using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using api.Options;
 
namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JwtController : ControllerBase
    {

        private readonly UserManager<User> userManager;
        private readonly PII_DBContext dbContext;
        private readonly Mapper mapper;
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtController(IOptions<JwtIssuerOptions> jwtOptions, PII_DBContext dbContext, UserManager<User> userManager )
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.userManager = userManager;
            mapper = new Mapper();
            _jwtOptions = jwtOptions.Value;
        }

        [HttpGet]
        public string Get()
        {
            return "Jwt Get";
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDTO model) {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            User customerFound = await userManager.FindByNameAsync(model.Username);
            bool isPasswordValid = await _userMgr.CheckPasswordAsync(customerFound, model.Password);
            if (customerFound == null || !isPasswordValid)
                return Unauthorized();

            var roles = await _userMgr.GetRolesAsync(customerFound);
            if(roles == null)
                return Unauthorized();

            IEnumerable<Claim> claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, customerFound.Username),
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

            var response = new {
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            return Ok(response);
        }

        private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() -
                            new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                            .TotalSeconds);
    }
}