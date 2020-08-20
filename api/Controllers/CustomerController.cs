using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    [ApiController]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class CustomerController : ApiController
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public CustomerController(PII_DBContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<User> userManager) : base(dbContext)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<CustomerDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> Get()
        {

            IEnumerable<User> users = await userManager.GetUsersInRoleAsync("CUSTOMER");

            if (!users.Any())
                return NotFound("No customer found");

            IEnumerable<CustomerDTO> customerDTOs = users.Select(x => Mapper.MapCustomerToDTO(x));

            return Ok(customerDTOs);
        }

        [HttpGet("{username}")]
        [ProducesResponseType(typeof(CustomerDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> Get([FromRoute] string username)
        {

            User user = await userManager.FindByNameAsync(username);

            if (user == null)
                return NotFound("No customer found");

            CustomerDTO customerDTO = Mapper.MapCustomerToDTO(user);

            return Ok(customerDTO);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] CustomerDTO customerDTO)
        {

            User customerFound = await userManager.FindByNameAsync(customerDTO.Username);

            if (customerFound != null)
                return BadRequest("Username already exist");


            User isEmailAvailib = await userManager.FindByEmailAsync(customerDTO.Email);
            if (isEmailAvailib != null)
                return BadRequest("Email is already in use");

            customerDTO.PhoneNumber = customerDTO.PhoneNumber.Trim();

            if (customerDTO.PhoneNumber != null)
            {
                User isPhoneAvailib = GetPII_DBContext().User.Where(u => u.PhoneNumber == customerDTO.PhoneNumber).FirstOrDefault();
                if (isPhoneAvailib != null)
                    return BadRequest("Phone number is already in use");
            }

            User newUser = Mapper.MapCustomerDToToCustomerModel(customerDTO);

            IdentityResult result = await userManager.CreateAsync(newUser, customerDTO.CustomerPassword);
            //Check in startup file more efficient ? 
            if (!await roleManager.RoleExistsAsync("CUSTOMER"))
                await roleManager.CreateAsync(new IdentityRole("CUSTOMER"));

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, "CUSTOMER");
                return Created("Customer created with success", null);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, CUSTOMER")]
        public async Task<ActionResult> Put([FromBody] CustomerDTO customerDTO)
        {
            User customerFound = await userManager.FindByNameAsync(customerDTO.Username);

            if (customerFound == null)
                return NotFound("Customer does not exist");

            User isEmailAvailib = await userManager.FindByEmailAsync(customerDTO.Email);
            if (isEmailAvailib != null)
                return BadRequest("email is already in use");

            if (customerDTO.PhoneNumber != null && customerDTO.PhoneNumber != customerFound.PhoneNumber)
            {
                User isPhoneAvailib = GetPII_DBContext().User.Where(u => u.PhoneNumber == customerDTO.PhoneNumber).FirstOrDefault();
                if (isPhoneAvailib != null)
                    return BadRequest("phone number is already in use");
            }

            try
            {
                customerFound = Mapper.MapCustomerDToToCustomerModel(customerDTO);



                await userManager.UpdateAsync(customerFound);
                //dbContext.Entry(customerFound).Property("RowVersion").OriginalValue;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                return Conflict("Conflict detected, transation cancel");
            }

            return Ok("Customer updated with success");
        }

        [HttpDelete("{customerId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public async Task<ActionResult> Delete([FromRoute] string customerId)
        {
            User customerFound = await userManager.FindByIdAsync(customerId);

            if (customerFound == null)
                return NotFound("Customer does not exist");

            await userManager.DeleteAsync(customerFound);
            return Ok("Customer deleted with success");
        }
    }
}
