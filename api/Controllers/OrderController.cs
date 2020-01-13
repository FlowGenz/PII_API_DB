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
        [ProducesResponseType(typeof(IEnumerable<DressOrderDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DressOrder>>> Get()
        {
            IEnumerable<DressOrderDTO> dressOrderDTO = await dbContext.DressOrder
                .Include(u => u.User)
                .Include(o => o.OrderLine)
                .Include(o => o.OrderLine).ThenInclude(o => o.Dress)
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
            User user = await dbContext.User
                .Include(u => u.DressOrder)
                .ThenInclude(o => o.OrderLine)
                .ThenInclude(o => o.Dress)
                .FirstOrDefaultAsync(u => u.UserName == username);

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
        public async Task<ActionResult<string>> Post([FromBody] DressOrderDTO dressOrderDTO)
        {
            User customerFind = await userManager.FindByNameAsync(dressOrderDTO.CustomerName);
            if (customerFind == null)
                return BadRequest("Customer does not exist");

            /*DressOrder dressOrder = customerFind.DressOrder.FirstOrDefault(d => !d.IsValid);
            if (dressOrder != null)
                return Ok(dressOrder.Id);*/

            DressOrder dressOrder = new DressOrder();
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
        public async Task<ActionResult> Put([FromBody] DressOrderDTO dressOrderDTO)
        {
            User customerFind = await dbContext.User.Include(u => u.DressOrder).FirstOrDefaultAsync(u => u.Id == dressOrderDTO.CustomerId);
            if (customerFind == null)
                return BadRequest("Customer does not exist");

            DressOrder dressOrder = customerFind.DressOrder.FirstOrDefault(d => d.IsValid == false);
            if (dressOrder == null)
                return BadRequest("The customer do not have a order in use");

            if (dressOrderDTO.IsValid && (dressOrderDTO.DeliveryDate == null || dressOrderDTO.BillingDate == null))
                return BadRequest("The order is not valid");

            //A mettre dans mapper plus tard

            dressOrder.BillingAddress = dressOrderDTO.BillingAddress;
            dressOrder.BillingDate = dressOrderDTO.BillingDate;
            dressOrder.DeliveryAddress = dressOrderDTO.DeliveryAddress;
            dressOrder.DeliveryDate = dressOrderDTO.DeliveryDate;
            dressOrder.IsValid = dressOrderDTO.IsValid;
            HashSet<OrderLine> orderLines = new HashSet<OrderLine>();
            foreach (OrderLineDTO orderLineDTO in dressOrderDTO.OrderLines) {

                OrderLine orderLineFound = await dbContext.OrderLine.FirstOrDefaultAsync(l => l.DressId == orderLineDTO.DressId && l.DressOrderId == orderLineDTO.DressOrderId);
                if (orderLineFound == null)
                    orderLineFound = new OrderLine();

                orderLineFound.DateBeginLocation = orderLineDTO.DateBeginLocation;
                orderLineFound.DateEndLocation = orderLineDTO.DateEndLocation;
                orderLineFound.Dress = await dbContext.Dress.FirstOrDefaultAsync(d => d.Id == orderLineDTO.DressId);
                orderLineFound.DressOrder = await dbContext.DressOrder.FirstOrDefaultAsync(d => d.Id == orderLineDTO.DressOrderId);
                orderLineFound.DressOrderId = orderLineDTO.DressOrderId;
                orderLineFound.FinalPrice = orderLineDTO.FinalPrice;

                orderLines.Add(orderLineFound);
            }

            dressOrder.OrderLine = orderLines;

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

        [HttpDelete("{dressOrderId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ObjectResult>> Delete([FromRoute] string dressOrderId) {

            DressOrder dressOrderFound = await dbContext.DressOrder.FirstOrDefaultAsync(d => d.Id == dressOrderId);

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
