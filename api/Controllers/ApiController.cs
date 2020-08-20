using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API_DbAccess;
using DTO;

namespace api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly PII_DBContext dbContext;
        public ApiController(PII_DBContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public PII_DBContext GetPII_DBContext() {
            return dbContext;
        }
    }
}