using Database.Models;
using Services.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AccServices
{
    public class AccSvc
    {
        protected readonly AppDBContext _db = new AppDBContext();

        public List<RetData> GetAccs()
        {
            RetData retData = new RetData();

            var ret_data = from u in _db.BtUsers
                           join a in _db.BtAccs
                           on u.UserId equals a.UserId
                           select new RetData
                           {
                               Acc_Id = a.AccId,
                               User_Name = u.UserName,
                               User_Email = u.UserEmail,
                               User_Address = u.UserAddress,
                               User_Phone = u.UserPhone,
                               Acc_Balance = a.AccBalance,
                               Acc_Pin = a.AccPin
                           };

            return ret_data.ToList();
        }

        public List<RetData> GetAccById(int id)
        {
            RetData retData = new RetData();

            var ret_data = from u in _db.BtUsers
                           join a in _db.BtAccs
                           on u.UserId equals a.UserId
                           where a.AccId == id
                           select new RetData
                           {
                               Acc_Id = a.AccId,
                               User_Name = u.UserName,
                               User_Email = u.UserEmail,
                               User_Address = u.UserAddress,
                               User_Phone = u.UserPhone,
                               Acc_Balance = a.AccBalance,
                               Acc_Pin = a.AccPin
                           };

            return ret_data.ToList();
        }

        public AccData AccountData(BtUser user)
        {
            AccData data = new AccData()
            {
                User_Name = user.UserName,
                User_Email = user.UserEmail,
                User_Address = user.UserAddress,
                User_Phone = user.UserPhone,
                Acc_Balance = 1000,
                Acc_Pin = "111222"
            };

            _db.BtUsers.Add(user);
            _db.SaveChanges();

            var item = _db.BtUsers
              .OrderByDescending(x => x.UserId)
              .FirstOrDefault();
            BtAcc bt = new BtAcc();
            bt.UserId = item!.UserId;
            bt.AccBalance = 1000;
            bt.AccPin = "1111";

            _db.BtAccs.Add(bt);
            _db.SaveChanges();

            return data;
        }

        public string UpdatePin(int id,string pin) 
        {
            if (pin != null && id != 0)
            {
                var item = _db.BtAccs.Where(x=>x.AccId==id).FirstOrDefault();
                if (item is null) return "Data not Exists";
                item.AccPin = pin;
                _db.SaveChanges();
                return "Update Success";
            } 
            else
            {
                return "Bad Request";
            }
        }

        public string Transfer(int fid,int tid,int amt)
        {
            var sender_data = _db.BtAccs.Where(x => x.AccId == fid).FirstOrDefault();
            if (sender_data is null) return "Account Not Exists.";


            var recvr_data = _db.BtAccs.Where(x => x.AccId == tid).FirstOrDefault();
            if (recvr_data is null) return "Account Not Exists.";

            if (amt > 0)
            {
                var sender_bal = sender_data.AccBalance;
                if (amt > sender_bal) return "OS Amount is not Sufficient.";

                sender_bal = sender_bal - amt;
                sender_data.AccBalance = sender_bal;
                _db.SaveChanges();

                var recvr_bal = recvr_data.AccBalance;
                recvr_bal += amt;
                recvr_data.AccBalance = recvr_bal;
                _db.SaveChanges();

                int tr_id = 0;
                var itemtran = _db.BtTrans
                .OrderByDescending(x => x.TranId)
                .FirstOrDefault();

                if (itemtran is not null) tr_id = Convert.ToInt32(itemtran.TranId) + 2;

                BtTran tran = new BtTran();
                tran.TranId = tr_id.ToString();
                tran.TranFrAccId = fid;
                tran.TranToAccId = tid;
                tran.Amount = amt;
                tran.TranDate = DateTime.Now;
                tran.TranSts = 1;
                _db.BtTrans.Add(tran);
                _db.SaveChanges();

                return "Transfer Success";
            }
            return "Bad Request";
        }
    }
}
