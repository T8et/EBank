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

        [HttpPut]
        public IActionResult UpdatePin(int id, string pin)
        {
            string item = _service.UpdatePin(id, pin);
            return Ok(item);
        }

        [HttpPost("Transfer")]
        public IActionResult Transfer(int fid, int tid, int amt)
        {
            string item = _service.Transfer(fid, tid, amt);
            return Ok(item);
        }

        [HttpPost("Deposit")]
        public IActionResult Deposit(int id, int amt)
        {
            string item = _service.Deposit(id, amt);
            return Ok(item);
        }

        [HttpPost("Withdrawl")]
        public IActionResult WithDrawl(int id, int amt)
        {
            string item = _service.Withdrawl(id, amt);
            return Ok(item);
        }
    }
}
