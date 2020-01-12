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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using DTO;
using Microsoft.AspNetCore.Identity;

namespace api.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ApiController
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly PII_DBContext dbContext;
        private readonly Mapper mapper;

        public CustomerController(PII_DBContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<User> userManager) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.roleManager = roleManager;
            this.userManager = userManager;
            mapper = new Mapper();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> Get()
        {

            IEnumerable<User> users = await userManager.GetUsersInRoleAsync("CUSTOMER");

            if (!users.Any())
                return NotFound("No customer found");

            IEnumerable<CustomerDTO> customerDTOs = users.Select(x => mapper.MapCustomerToDTO(x));

            return Ok(customerDTOs);
        }

        [HttpGet("{username}")]
        [ProducesResponseType(typeof(CustomerDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> Get([FromRoute] string username)
        {

            User user = await userManager.FindByNameAsync(username);

            if (user == null)
                return NotFound("No customer found");

            CustomerDTO customerDTO =  mapper.MapCustomerToDTO(user);

            return Ok(customerDTO);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Post([FromBody] CustomerDTO customerDTO)
        {
            User customerFound = await userManager.FindByNameAsync(customerDTO.Username);

            if (customerFound != null)
                return BadRequest("Username already exist");

            User newUser = new User();

            newUser.UserName = customerDTO.Username;
            newUser.UserAddress = customerDTO.CustomerAddress;
            newUser.LoyaltyPoints = customerDTO.LoyaltyPoints;
            newUser.LastName = customerDTO.LastName;
            newUser.FirstName = customerDTO.FirstName;
            newUser.Email = customerDTO.Email;
            newUser.PhoneNumber = customerDTO.PhoneNumber;

            
            
            User isEmailAvailib = await userManager.FindByEmailAsync(customerDTO.Email);
            if(isEmailAvailib != null)
                return BadRequest("email is already in use");

            if (customerDTO.PhoneNumber != null && customerDTO.PhoneNumber.Trim() != "") {
                User isPhoneAvailib = dbContext.User.Where(u => u.PhoneNumber == customerDTO.PhoneNumber).FirstOrDefault();
                if (isPhoneAvailib != null)
                    return BadRequest("phone number is already in use");
            }

            IdentityResult result = await userManager.CreateAsync(newUser, customerDTO.CustomerPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors.First().Description);
            
            await roleManager.CreateAsync(new IdentityRole("CUSTOMER"));
            result = await userManager.AddToRoleAsync(newUser, "CUSTOMER");

            if (!result.Succeeded)
                return BadRequest(result.Errors.First().Description);

            return Created("Customer created with success", newUser.Id);
        }

        [HttpPut]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Put([FromBody] CustomerDTO customerDTO)
        {

            User customerFound = await userManager.FindByNameAsync(customerDTO.Username);

            if (customerFound == null)
                return NotFound("Customer does not exist");

            User isEmailAvailib = await userManager.FindByEmailAsync(customerDTO.Email);
            if (isEmailAvailib != null)
                return BadRequest("email is already in use");

            customerFound.FirstName = customerDTO.FirstName;
            customerFound.LastName = customerDTO.LastName;
            customerFound.PasswordHash = customerDTO.CustomerPassword;
            customerFound.Email = customerDTO.Email;
            customerFound.UserName = customerDTO.Username;
            customerFound.UserAddress = customerDTO.CustomerAddress;
            customerFound.LoyaltyPoints = customerDTO.LoyaltyPoints;
            customerFound.PhoneNumber = customerDTO.PhoneNumber;

            IdentityResult result = await userManager.UpdateAsync(customerFound);

            if(result.Succeeded)
                return Ok("Customer updated with success");
            return BadRequest();
        }

        [HttpDelete("{customerId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Delete([FromRoute] string customerId)
        {
            User customerFound = await userManager.FindByIdAsync(customerId);
         
            
            if (customerFound == null)
                return NotFound("Customer does not exist");

            //verification néccaisaire ?
            bool isCustomer = await userManager.IsInRoleAsync(customerFound, "CUSTOMER");

            if (!isCustomer)
                return Unauthorized("Action unauthorized"); 

            IdentityResult result = await userManager.DeleteAsync(customerFound);

            if (result.Succeeded)
                return Ok("Customer deleted with success");
            return BadRequest();

        }
    }
}
