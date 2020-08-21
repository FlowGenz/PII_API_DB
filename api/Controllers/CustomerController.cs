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
using api.DTO;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class CustomerController : DefaultController
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public CustomerController(PII_DBContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<User> userManager) : base(dbContext)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));

        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> Get()
        {

            IEnumerable<User> users = await userManager.GetUsersInRoleAsync("CUSTOMER");

            if (!users.Any())
                return NotFound("No customer found");

            IEnumerable<CustomerDTO> customerDTOs = users.Select(x => Mapper.MapCustomerModelToCustomerDTO(x));

            return Ok(customerDTOs);
        }

        [HttpGet("{pageIndex}/{pageSize}")]
        [ProducesResponseType(typeof(PaginationCustomerDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, CUSTOMER")]
        public async Task<ActionResult<PaginationCustomerDTO>> GetAllCustomerWithPagination([FromRoute]int pageIndex = 0, [FromRoute] int pageSize = 6)
        {
            IEnumerable<User> users = await userManager.GetUsersInRoleAsync("CUSTOMER");

            IEnumerable<CustomerDTO> customersDTO = users
                .OrderBy(c => c.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(c => Mapper.MapCustomerModelToCustomerDTO(c));

            /*
            IEnumerable<CustomerDTO> customersDTO = userManager.GetUsersInRoleAsync("CUSTOMER").Result
                .OrderBy(c => c.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(x => Mapper.MapCustomerModelToCustomerDTO(x))
                .ToList();
            */

            int count = await GetPII_DBContext().User.CountAsync();

            if (count > 0 && customersDTO.Any())
                return Ok(new PaginationCustomerDTO(customersDTO, pageIndex, pageSize, (int)Math.Ceiling(count / (double)pageSize)));
            return NotFound("No customer found");
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

            CustomerDTO customerDTO = Mapper.MapCustomerModelToCustomerDTO(user);

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

            User newUser = Mapper.MapCustomerDtoToCustomerModel(customerDTO);

            IdentityResult result = await userManager.CreateAsync(newUser, customerDTO.CustomerPassword);
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
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, CUSTOMER")]
        public async Task<ActionResult> Put([FromBody] CustomerDTO customerDTO)
        {
            User customerFound = await userManager.FindByIdAsync(customerDTO.Id);

            if (customerFound == null)
                return NotFound("Customer does not exist");

            try
            {
                //customerFound = Mapper.MapCustomerDtoToCustomerModel(customerDTO);


                customerFound.UserName = customerDTO.Username;
                customerFound.UserAddress = customerDTO.CustomerAddress;
                customerFound.LoyaltyPoints = customerDTO.LoyaltyPoints;
                customerFound.LastName = customerDTO.LastName;
                customerFound.FirstName = customerDTO.FirstName;
                customerFound.Email = customerDTO.Email;
                customerFound.PhoneNumber = customerDTO.PhoneNumber;

                //

                GetPII_DBContext().Entry(customerFound).Property("RowVersion").OriginalValue = customerDTO.RowVersion;
                await GetPII_DBContext().SaveChangesAsync();
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public async Task<ActionResult> Delete([FromRoute] string customerId)
        {
            User customerFound = await userManager.FindByIdAsync(customerId);

            if (customerFound == null)
                return NotFound("Customer does not exist");

            await userManager.DeleteAsync(customerFound);
            await GetPII_DBContext().SaveChangesAsync();
            return Ok("Customer deleted with success");
        }
    }
}
