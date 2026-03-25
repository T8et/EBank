using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoAccController : ControllerBase
    {
        public class ReturnData
        {
            public int Acc_Id { get; set; }

            public string? User_Name { get; set; }

            public string? User_Email { get; set; }

            public string? User_Address { get; set; }

            public string? User_Phone { get; set; }

            public int? Acc_Balance { get; set; }

            public string? Acc_Pin { get; set; }
        }

        public class ReturnData1
        {
            public int User_Id { get; set; }
        }

        protected readonly string _connection = "Data Source=.;Initial Catalog=MeBank;User Id=sa;Password=p@ssw0rd;Trust Server Certificate=True";

        [HttpGet("acc_all")]
        public IActionResult GetAcc()
        {
            SqlConnection con = new SqlConnection(_connection);
            con.Open();

            List<ReturnData> UserData = new List<ReturnData>();

            string query = @"SELECT 
                                A.Acc_Id as Acc_Id,
                                U.User_Id as User_Id,
                                U.User_Name as User_Name, 
                                U.User_Email as User_Email,
                                U.User_Address as User_Address, 
                                U.User_Phone as User_Phone,
                                A.Acc_Balance as Acc_Balance
                                FROM BT_User U
                                JOIN BT_Acc A
                                ON U.User_Id = A.User_Id";

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) 
            {
                UserData.Add(new ReturnData
                {
                    Acc_Id = Convert.ToInt32(reader["User_Id"]),
                    User_Name = Convert.ToString(reader["User_Name"]),
                    User_Email = Convert.ToString(reader["User_Email"]),
                    User_Address = Convert.ToString(reader["User_Address"]),
                    User_Phone = Convert.ToString(reader["User_Phone"]),
                    Acc_Balance = Convert.ToInt32(reader["Acc_Balance"]),
                    Acc_Pin = "******"
                });
            }
            reader.Close();

            con.Close();
            return Ok(UserData);
        }

        [HttpGet("acc_id")]
        public IActionResult GetAcc(int uid)
        {
            SqlConnection con = new SqlConnection(_connection);
            con.Open();

            List<ReturnData> UserData = new List<ReturnData>();

            string query = @"SELECT 
                                A.Acc_Id as Acc_Id,
                                U.User_Id as User_Id,
                                U.User_Name as User_Name, 
                                U.User_Email as User_Email,
                                U.User_Address as User_Address, 
                                U.User_Phone as User_Phone,
                                A.Acc_Balance as Acc_Balance
                                FROM BT_User U
                                JOIN BT_Acc A
                                ON U.User_Id = A.User_Id
                                Where A.acc_Id = @uid";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@uid", uid);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                UserData.Add(new ReturnData
                {
                    Acc_Id = Convert.ToInt32(reader["User_Id"]),
                    User_Name = Convert.ToString(reader["User_Name"]),
                    User_Email = Convert.ToString(reader["User_Email"]),
                    User_Address = Convert.ToString(reader["User_Address"]),
                    User_Phone = Convert.ToString(reader["User_Phone"]),
                    Acc_Balance = Convert.ToInt32(reader["Acc_Balance"]),
                    Acc_Pin = "******"
                });
            }

            con.Close();
            return Ok(UserData);
        }

        [HttpPatch("acc_id/update")]
        public IActionResult UpdateAcc(int uid, BtUser userData)
        {
            SqlConnection con = new SqlConnection( _connection);
            con.Open();

            GetAcc(uid);
            if (userData is null) return BadRequest("Acc Data Not Found");

            string updquery = @"UPDATE [dbo].[BT_User]
                                SET [User_Name] = @username
                                    ,[User_Phone] = @userphone
                                    ,[User_Email] = @useremail
                                    ,[User_Address] = @useraddr where User_Id = @uid";

            SqlCommand cmd = new SqlCommand(updquery, con);

            if (userData.UserName != null) cmd.Parameters.AddWithValue("@username", userData.UserName);
            if (userData.UserPhone != null) cmd.Parameters.AddWithValue("@userphone", userData.UserPhone);
            if (userData.UserEmail != null) cmd.Parameters.AddWithValue("@useremail", userData.UserEmail);
            if (userData.UserAddress != null) cmd.Parameters.AddWithValue("@useraddr", userData.UserAddress);
            cmd.Parameters.AddWithValue("@uid", uid);
            cmd.ExecuteNonQuery();

            con.Close();

            return Ok(userData);
        }

        //Create Account with ADO.Net
        [HttpPost("acc_id/create")]
        public IActionResult UpdatePin(BtUser acc_data)
        { 
            SqlConnection conn = new SqlConnection( _connection);
            conn.Open();
            List<ReturnData1> UserData = new List<ReturnData1>();
            string basequery = @"INSERT INTO [dbo].[BT_User]
                                   ([User_Name]
                                   ,[User_Phone]
                                   ,[User_Email]
                                   ,[User_Address])
                                 VALUES
                                   (@username
                                   ,@userphone
                                   ,@useremail
                                   ,@useradd)";

            SqlCommand bcmd = new SqlCommand(basequery, conn);
            if (acc_data.UserName is null) return BadRequest();
            bcmd.Parameters.AddWithValue("@username",acc_data.UserName);
            if (acc_data.UserPhone is null) return BadRequest();
            bcmd.Parameters.AddWithValue("@userphone", acc_data.UserPhone);
            bcmd.Parameters.AddWithValue("@useremail", acc_data.UserEmail);
            bcmd.Parameters.AddWithValue("@useradd", acc_data.UserAddress);
            int result = bcmd.ExecuteNonQuery();
            if(result == 0) return BadRequest();

            string exequery = "Select User_Id from BT_User where User_Phone=@phone";
            SqlCommand ecmd = new SqlCommand(@exequery, conn);
            ecmd.Parameters.AddWithValue("@phone", acc_data.UserPhone);
            var re = ecmd.ExecuteScalar();
            int aid = 0;
            if(re is not null)
            {
                aid = Convert.ToInt32(re);
            }

            string query = @"INSERT INTO [dbo].[BT_Acc]
                                    ([User_Id]
                                   ,[Acc_Balance]
                                   ,[Acc_Pin])
                             VALUES
                                    (@uid,
                                   1000
                                   ,@pin)";


            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@uid", aid);
            cmd.Parameters.AddWithValue("@pin", "123456");
            cmd.ExecuteNonQuery();

            conn.Close();

            return Ok("Account Created!");
        }

    }
}
