using Microsoft.AspNetCore.Mvc;
using System;

namespace PW.WebUI.Controllers
{
    [Route("api/check")]
    public class CheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            throw new Exception("message");
        }

        [HttpPost]
        public IActionResult Sent()
        {
            throw new Exception("message");
        }
    }
}