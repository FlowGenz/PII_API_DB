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

namespace api.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ApiController
    {


        private readonly PII_DBContext dbContext;
        private readonly Mapper mapper;

        public CustomerController(PII_DBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            mapper = new Mapper();
        }

        [HttpGet]
        [ProducesResponseType(typeof(CustomerDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> Get()
        {
            IEnumerable<CustomerDTO> customerDTO = await dbContext.User
                .Select(x => mapper.MapCustomerToDTO(x))
                .ToListAsync();

            if (!customerDTO.Any())
                return NotFound("No customer found");

            return Ok(customerDTO);
        }
     
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Post([FromBody] User customer)
        {
            User customerFound = await dbContext.User.FirstOrDefaultAsync(c => c.UserName == customer.UserName);

            if (customerFound != null)
                return BadRequest("Username already exist");

            try {
                dbContext.Add<User>(customer);
                dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex) {
                var entry = ex.Entries.Single();
                entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                return Conflict("Conflict detected, transation cancel");
            }

            return Created("Customer created with success", customer.Id);
        }

        // pour les put revoir si ils recevoient des models ou des DTO 
        [HttpPut]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Put([FromBody] CustomerDTO customer)
        {

            User customerFound = await dbContext.User.FirstOrDefaultAsync(c => c.UserName == customer.Username);

            if (customerFound == null)
                return NotFound("Customer does not exist");

            try
            {
                customerFound.FirstName = customer.FirstName;
                customerFound.LastName = customer.LastName;
                customerFound.PasswordHash = customer.CustomerPassword;
                customerFound.Email = customer.Email;
                customerFound.UserName = customer.Username;
                customerFound.UserAddress = customer.CustomerAddress;
                customerFound.LoyaltyPoints = customer.LoyaltyPoints;
                customerFound.PhoneNumber = customer.PhoneNumber;
                dbContext.User.Update(customerFound);
                dbContext.SaveChanges();
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
        public async Task<ActionResult> Delete(int customerId)
        {
            User customerFound = await dbContext.User.FindAsync(customerId);

            if (customerFound == null)
                return NotFound("Customer does not exist");
            try
            {
                dbContext.User.Remove(customerFound);
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                return Conflict("Conflict detected, transation cancel");
            }


            return Ok("Customer deleted with success");
        }

    }
}
