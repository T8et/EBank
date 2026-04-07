using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.AccServices;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccSvcsController : ControllerBase
    {
        protected readonly AccSvc _service = new AccSvc();

        [HttpGet]
        public IActionResult GetAccs()
        {
            var item = _service.GetAccs();

            if(item is not null) return Ok(item);

            return BadRequest();
        }

        [HttpGet("Id")]
        public IActionResult GetAccById(int id)
        {
            var item = _service.GetAccById(id);

            if(item is null) return BadRequest();

            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateAccount(BtUser user) 
        {
            _service.AccountData(user);

            return Ok(user);
        }
    }
}
