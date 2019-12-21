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
        private readonly Mapper mapper;

        public CustomerController(PII_DBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            mapper = new Mapper();
        }

        /// <summary>
        /// Get all customer.
        /// </summary>
        /// <response code="201">Returns an IEnumerable of all customers</response>
        /// <response code="400">If the item is null</response>            
        [HttpGet]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(String), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(String), StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CustomerDTO>> Get()
        {
            IEnumerable<User> customers = dbContext.User.ToList(); //#warning utiliser l'async tastk & await

            if (customers.Any()) {
                List<CustomerDTO> customersDTO = new List<CustomerDTO>();
                foreach (User customer in customers) {
                    CustomerDTO dto = mapper.MapCustomerToDTO(customer);
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
        [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)] // si on renvoie juste l'id alors vaut changer le Customer en int
        [ProducesResponseType(typeof(String), StatusCodes.Status400BadRequest)]
        public ActionResult<User> Post([FromBody] User customer) { 
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
