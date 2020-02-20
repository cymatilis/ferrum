using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ferrum.Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : Controller
    {
        public IActionResult Get()
        {
            return new JsonResult(new { Message = "You have reached the Ferrum Gateway." });
        }

        [Route("error")]
        public IActionResult Error()
        {
            throw new Exception("Simulated Exception");
        }
    }
}