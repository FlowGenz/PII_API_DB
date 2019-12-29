using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_DbAccess;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;

namespace api.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ApiController
    {

        private readonly PII_DBContext dbContext;
        private readonly Mapper mapper;
        private readonly UserManager<User> userManager;

        public OrderController(PII_DBContext dbContext, UserManager<User> userManager) : base(dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.userManager = userManager;
            mapper = new Mapper();
        }

        [HttpGet]
        [ProducesResponseType(typeof(DressOrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DressOrder>>> Get()
        {
            IEnumerable<DressOrderDTO> dressOrderDTO = await dbContext.DressOrder
                .Select(x => mapper.MapOrderToDTO(x))
                .ToListAsync();

            if (!dressOrderDTO.Any())
                return NotFound("No order found");

            return Ok(dressOrderDTO);
        }

        [HttpGet("{username}")]
        [ProducesResponseType(typeof(DressOrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DressOrder>>> Get([FromRoute] string username)
        {
            User user = await userManager.FindByNameAsync(username);
            if (User == null) {
                return BadRequest("Customer does not exist");
            }

            DressOrder dressOrder = user.DressOrder.FirstOrDefault(d => !d.IsValid);

            if (dressOrder == null)
                return NotFound("No order found");

            DressOrderDTO dressOrderDTO = mapper.MapOrderToDTO(dressOrder);

            return Ok(dressOrderDTO);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ObjectResult>> Post([FromBody] DressOrder orderLine)
        {

            User customerFind = await userManager.FindByNameAsync(orderLine.User.UserName);
            if (customerFind == null)
                return BadRequest("Customer does not exist");

            DressOrder dressOrder = customerFind.DressOrder.FirstOrDefault(d => !d.IsValid);
            if (dressOrder != null)
                return BadRequest("The customer already have a order in use");

            dressOrder = new DressOrder();
            dressOrder.BillingAddress = customerFind.UserAddress;
            dressOrder.DeliveryAddress = customerFind.UserAddress;
            dressOrder.User = customerFind;
            customerFind.DressOrder.Add(dressOrder);

            try
            {
                await dbContext.DressOrder.AddAsync(dressOrder);
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                return Conflict("Conflict detected, transation cancel");
            }

            return Created("Dress order added with success", dressOrder.Id);
        }

        [HttpPut]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Put([FromBody] DressOrder orderLine)
        {
            User customerFind = await userManager.FindByNameAsync(orderLine.User.UserName);
            if (customerFind == null)
                return BadRequest("Customer does not exist");

            DressOrder dressOrder = customerFind.DressOrder.FirstOrDefault(d => !d.IsValid);
            if (dressOrder == null)
                return BadRequest("The customer already have not a order in use");

            try
            {
                dbContext.DressOrder.Update(dressOrder);
                dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                return Conflict("Conflict detected, transation cancel");
            }

            return Ok("Order updated with success");
        }

        [HttpDelete]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ObjectResult>> Delete([FromBody] string dressOrder) {

            DressOrder dressOrderFound = await dbContext.DressOrder.FirstOrDefaultAsync(d => d.Id == dressOrder);

            if (dressOrderFound == null)
                return BadRequest("order does not exist");

            try
            {
                dbContext.DressOrder.Remove(dressOrderFound);
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                return Conflict("Conflict detected, transation cancel");
            }

            return Ok("Dress order added with success");

        }
    }
}
