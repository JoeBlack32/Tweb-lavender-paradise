    using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.BusinessLogic.Core.User;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;
using Tweb_lavender_paradise.Domain.Enitities.User;


namespace Tweb_lavender_paradise.BusinessLogic.BLogic
{
    public class UserBL : UserApi, IUser
    {
        public bool IsValidSession(string key)
        {
            return IsValidSessionAction(key);
        }

        private readonly string _connectionString = "Data Source=LocalHost;Initial Catalog=LavenderParadise;Integrated Security=True;MultipleActiveResultSets=True;App=LavenderParadise";

        public bool AddUser(UserDBTable data)
        {
            AddUserApi(data);

            return true;
        }

        public void UpdatePassword(int userId, string newPassword)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE Users SET PasswordHash = @Password WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Password", newPassword);
                cmd.Parameters.AddWithValue("@Id", userId);
                cmd.ExecuteNonQuery();
            }
        }

    }
}
