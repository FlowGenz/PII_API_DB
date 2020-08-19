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
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] CustomerDTO customerDTO)
        {

            User customerFound = await userManager.FindByNameAsync(customerDTO.Username);

            if (customerFound != null)
                return BadRequest("Username already exist");
            
            
            User isEmailAvailib = await userManager.FindByEmailAsync(customerDTO.Email);
            if(isEmailAvailib != null)
                return BadRequest("email is already in use");

            customerDTO.PhoneNumber = customerDTO.PhoneNumber.Trim();

            if (customerDTO.PhoneNumber != null) {
                User isPhoneAvailib = dbContext.User.Where(u => u.PhoneNumber == customerDTO.PhoneNumber).FirstOrDefault();
                if (isPhoneAvailib != null)
                    return BadRequest("phone number is already in use");
            }

            User newUser = mapper.MapCustomerDToToCustomerModel(customerDTO);

            await userManager.CreateAsync(newUser, customerDTO.CustomerPassword);

            // regarder de nouveau pour ces deux lignes de code

            //await roleManager.CreateAsync(new IdentityRole("CUSTOMER"));
            await userManager.AddToRoleAsync(newUser, "CUSTOMER");

            
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

            if (customerDTO.PhoneNumber != null && customerDTO.PhoneNumber != customerFound.PhoneNumber)
            {
                User isPhoneAvailib = dbContext.User.Where(u => u.PhoneNumber == customerDTO.PhoneNumber).FirstOrDefault();
                if (isPhoneAvailib != null)
                    return BadRequest("phone number is already in use");
            }

            customerFound = mapper.MapCustomerDToToCustomerModel(customerDTO);

            try
            {
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
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
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
