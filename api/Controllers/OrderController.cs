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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using api.DTO;

namespace api.Controllers
{
    
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : DefaultController
    {

        private readonly UserManager<User> userManager;

        public OrderController(PII_DBContext dbContext, UserManager<User> userManager) : base(dbContext)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DressOrderDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<DressOrder>>> Get()
        {
            IEnumerable<DressOrderDTO> dressOrderDTO = await GetPII_DBContext().DressOrder
                .Include(o => o.User)
                .Include(o => o.OrderLine)
                .Include(o => o.OrderLine).ThenInclude(o => o.Dress)
                .Select(x => Mapper.MapDressOrderModelToDressOrderDTO(x))
                .ToListAsync();

            if (!dressOrderDTO.Any())
                return NotFound("No order found");

            return Ok(dressOrderDTO);
        }

        //TODO héritage

        [HttpGet("{pageIndex}/{pageSize}")]
        [ProducesResponseType(typeof(PaginationDressOrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, CUSTOMER")]
        public async Task<ActionResult<PaginationDressOrderDTO>> GetAllOrdersWithPagination([FromRoute]int pageIndex = 0, [FromRoute] int pageSize = 6)
        {


            IEnumerable<DressOrderDTO> dressesOrderDTO = await GetPII_DBContext().DressOrder
                .Include(o => o.User)
                .OrderBy(o => o.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(x => Mapper.MapDressOrderModelToDressOrderDTO(x))
                .ToListAsync();

            int count = await GetPII_DBContext().DressOrder.CountAsync();

            if (count > 0 && dressesOrderDTO.Any())
                return Ok(new PaginationDressOrderDTO(dressesOrderDTO, pageSize, pageIndex, (int)Math.Ceiling(count / (double)pageSize)));
            return NotFound("No order found");
        }

        /*[HttpGet("{username}")]
        [ProducesResponseType(typeof(DressOrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, CUSTOMER")]
        public async Task<ActionResult<IEnumerable<DressOrder>>> Get([FromRoute] string username)
        {
            User user = await GetPII_DBContext().User
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

            DressOrderDTO dressOrderDTO = Mapper.MapDressOrderModelToDressOrderDTO(dressOrder);
            return Ok(dressOrderDTO);
        }*/

        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(DressOrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, CUSTOMER")]
        public async Task<ActionResult<IEnumerable<DressOrderDTO>>> GetOneDressOrder([FromRoute] string orderId)
        {

            DressOrder dressOrder = await GetPII_DBContext().DressOrder
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (dressOrder == null)
                return NotFound("No order found");

            DressOrderDTO dressOrderDTO = Mapper.MapDressOrderModelToDressOrderDTO(dressOrder);

            return Ok(dressOrderDTO);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CUSTOMER")]
        public async Task<ActionResult<string>> Post([FromBody] DressOrderDTO dressOrderDTO)
        {

            if (dressOrderDTO.IsValid != false) {
                return BadRequest("A new order can not have the attribute isValid set at other value then false");
            }
            
            User customerFind = await userManager.FindByNameAsync(dressOrderDTO.CustomerName);
            if (customerFind == null)
                return BadRequest("Customer does not exist");

            DressOrder dressOrder = Mapper.MapDressOrderDtoToDressOrderModel(dressOrderDTO);
            customerFind.DressOrder.Add(dressOrder);

            await GetPII_DBContext().DressOrder.AddAsync(dressOrder);
            await GetPII_DBContext().SaveChangesAsync();
            return Created("Dress order added with success", dressOrder.Id);
        }

        [HttpPut]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
        public async Task<ActionResult> Put([FromBody] DressOrderDTO dressOrderDTO)
        {

            if (dressOrderDTO.OrderLines.Count < 1)
                return BadRequest("An order can not be update without orderLines");

            User customerFind = await GetPII_DBContext().User.Include(u => u.DressOrder).FirstOrDefaultAsync(u => u.Id == dressOrderDTO.CustomerId);
            if (customerFind == null)
                return BadRequest("Customer does not exist");

            DressOrder dressOrderFound = customerFind.DressOrder.FirstOrDefault(d => d.IsValid == false);
            if (dressOrderFound == null)
                return BadRequest("The customer do not have a order in use");

            if (dressOrderDTO.IsValid && (dressOrderDTO.DeliveryDate == null || dressOrderDTO.BillingDate == null))
                return BadRequest("The order is not valid");

            try
            {

                //

                dressOrderFound.BillingAddress = dressOrderDTO.BillingAddress;
                dressOrderFound.BillingDate = dressOrderDTO.BillingDate;
                dressOrderFound.DeliveryAddress = dressOrderDTO.DeliveryAddress;
                dressOrderFound.DeliveryDate = dressOrderDTO.DeliveryDate;
                dressOrderFound.IsValid = dressOrderDTO.IsValid;

                //

                GetPII_DBContext().Entry(dressOrderFound).Property("RowVersion").OriginalValue = dressOrderDTO.RowVersion;
                GetPII_DBContext().SaveChanges();
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN, CUSTOMER")]
        public async Task<ActionResult> Delete([FromRoute] string dressOrderId) {

            DressOrder dressOrderFound = await GetPII_DBContext().DressOrder.FirstOrDefaultAsync(d => d.Id == dressOrderId);

            if (dressOrderFound == null)
                return BadRequest("order does not exist");

            //Async ?
            GetPII_DBContext().DressOrder.Remove(dressOrderFound);
            await GetPII_DBContext().SaveChangesAsync();

            return Ok("Dress order added with success");
        }
    }
}
