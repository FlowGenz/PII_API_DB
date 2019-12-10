using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using API_DbAccess;
using DTO;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ApiController
    {

        private readonly PII_DBContext dbContext;

        public CustomerController(PII_DBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet]
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

        [HttpPost]
        public ActionResult<Customer> Post([FromBody] Customer customer) {
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
