using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestAPI.Controllers
{
    [Route("acc/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        protected readonly AppDBContext _db = new AppDBContext();

        [HttpGet]
        public IActionResult GetAcc()
        {
            var item = from u in _db.BtUsers
                       join a in _db.BtAccs
                       on u.UserId equals a.UserId
                       select new
                       {
                           Account_Id = a.AccId,
                           User_Id = a.UserId,
                           User_name = u.UserName,
                           Account_Bal = a.AccBalance,
                           Pin = a.AccPin
                       };
            return Ok(item);
        }

        [HttpGet("{accId}")]
        public IActionResult GetAccById(int accId)
        {
            var item = from u in _db.BtUsers
                       join a in _db.BtAccs
                       on u.UserId equals a.UserId
                       where a.AccId == accId
                       select new
                       {
                           Account_Id = a.AccId,
                           User_Id = a.UserId,
                           User_name = u.UserName,
                           Account_Bal = a.AccBalance,
                           Pin = a.AccPin
                       };
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateUser(BtUser userdata)
        {
            _db.BtUsers.Add(userdata);
            _db.SaveChanges();

            BtAcc bt = new BtAcc();

            var item = _db.BtUsers
              .OrderByDescending(x => x.UserId)
              .FirstOrDefault();

            if (item is null) return BadRequest();
            bt.UserId = item.UserId;
            bt.AccBalance = 1000;
            _db.BtAccs.Add(bt);

            _db.SaveChanges();

            return Ok("Data Created Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult CreatePin(int id,string pin)
        {
            var item = _db.BtAccs.Where(x=>x.UserId==id).FirstOrDefault();

            if (item is null) return BadRequest();
            
            item.AccPin = pin;
            _db.SaveChanges();

            return Ok("Pin Updated Successfully");
        }

        [HttpPatch("{fid}")]
        public IActionResult Transfer(int fid, int tid, int amount)
        {
            var frAcc_data = _db.BtAccs.Where(x=>x.AccId==fid).FirstOrDefault();
            if(frAcc_data is null) return BadRequest();

            var toAcc_data = _db.BtAccs.Where(x => x.AccId == tid).FirstOrDefault();
            if (toAcc_data is null) return BadRequest();

            var balance = frAcc_data.AccBalance;
            balance = balance - amount;

            frAcc_data.AccBalance = balance;
            _db.SaveChanges();

            var nbalance = toAcc_data.AccBalance;
            nbalance += amount;

            toAcc_data.AccBalance = nbalance;
            _db.SaveChanges();

            var itemtran = _db.BtTrans
              .OrderByDescending(x => x.TranId)
              .FirstOrDefault();

            string tr_id = "0";
            if (itemtran is not null) tr_id = itemtran.TranId + "01";

            BtTran tran = new BtTran();
            tran.TranId = tr_id;
            tran.TranFrAccId = fid;
            tran.TranToAccId = tid;
            tran.Amount = amount;
            tran.TranDate = DateTime.Now;
            tran.TranSts = 1;
            _db.BtTrans.Add(tran);
            _db.SaveChanges();

            return Ok("Transaction Completed");

        }
    }
}
