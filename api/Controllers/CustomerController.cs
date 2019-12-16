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

namespace api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ApiController
    {

        private readonly PII_DBContext dbContext;

        public CustomerController(PII_DBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Get all customer.
        /// </summary>
        /// <response code="201">Returns an IEnumerable of all customers</response>
        /// <response code="400">If the item is null</response>            
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<CustomerDTO>> Get()
        {
            IEnumerable<Customer> customers = dbContext.Customer.Include(u => u.UsernameUserNavigation).ToList();

            if (customers.Any()) {
                List<CustomerDTO> customersDTO = new List<CustomerDTO>();
                foreach (Customer customer in customers) {
                    CustomerDTO dto = Mapper.MapCustomerToDTO(customer);
                    customersDTO.Add(dto);
                }
                return Ok(customersDTO);
            }
            return NotFound("No customer found");
        }

        /// <summary>
        /// Add a customer.
        /// </summary>
        /// <param name="customer"></param> 
        /// <response code="201">Returns the newly created Customer</response>
        /// <response code="400">If the customer is null</response>            
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Customer> Post([FromQuery] Customer customer) {
            //try {
                dbContext.Add<Customer>(customer);
                dbContext.SaveChanges();
            //} catch (DbUpdateException dbUpdateException) {
                // return BadRequest(dbUpdateException);
            //}
            return Created("PII_DB", customer);
        }
    }
}
