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
    }
}
