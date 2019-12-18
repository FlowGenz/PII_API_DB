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
    [Produces("application/json")]
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ApiController //#héritage pas utile
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
        [ProducesResponseType(StatusCodes.Status201Created)] //#warning Manque le type de retour, maus vais status code 200
        [ProducesResponseType(StatusCodes.BadRequest)] //#warning Mauvais status code 404
        public ActionResult<IEnumerable<CustomerDTO>> Get()
        {
            IEnumerable<User> customers = dbContext.User.ToList(); //#warning utiliser l'async tastk & await

            if (customers.Any()) {
                List<CustomerDTO> customersDTO = new List<CustomerDTO>();
                foreach (User customer in customers) {
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
        public ActionResult<User> Post([FromQuery] User customer) { //#warning Pourquoi FromQuery et pas FromBody ?
                                                                    //#warning Pourquoi dire que tu fais un actionResult sur User ? Pourquoi le dévoiler ?
            //try {
                dbContext.Add<User>(customer); //#warning ajouter une verification de la creation en transaction
                dbContext.SaveChanges();
            //} catch (DbUpdateException dbUpdateException) {
                // return BadRequest(dbUpdateException);
            //}
            return Created("PII_DB_IG", customer); //#warning renvoyer l'id, c'est inutile de renvoyer l'objet
        }
    }
}
