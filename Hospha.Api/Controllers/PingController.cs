using Hospha.DbModel;
using Hospha.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospha.Api.Controllers
{
    [ApiController]
    [Authorize(Model.Constants.Policy.adminPolicy)]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {

        private readonly ILogger<PingController> _logger;

        public PingController(ILogger<PingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Ping()
        {
            return Ok("Server is runing");
        }
    }
}
