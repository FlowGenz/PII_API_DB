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
    public class DefaultController : ControllerBase
    {
        private readonly PII_DBContext dbContext;
        public DefaultController(PII_DBContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        protected PII_DBContext GetPII_DBContext() {
            return dbContext;
        }
    }
}