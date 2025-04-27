using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.Domain.Models;

namespace Tweb_lavender_paradise.BusinessLogic.BLogic
{
    public class AuthServiceBL
    {
        private readonly List<UserModel> _users = new List<UserModel>
        {
            new UserModel { Id = 1, FirstName = "Admin", LastName = "Slava", Email = "admin@example.com", PasswordHash = "hashed_password", Role = "Admin" },
            new UserModel { Id = 2, FirstName = "User", LastName = "Slava", Email = "user@example.com", PasswordHash = "hashed_password", Role = "User" }
        };

        public UserModel Authenticate(string email, string password)
        {
            var user = _users.FirstOrDefault(u => u.Email == email);
            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                return user;
            }
            return null;
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // Логика проверки пароля (bcrypt или другой хеш)
            return password == storedHash;
        }

        public bool Register(UserModel newUser)
        {
            using (var connection = new SqlConnection("Data Source=LocalHost;Initial Catalog=LavenderParadise;Integrated Security=True;MultipleActiveResultSets=True;App=LavenderParadise"))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Users (FirstName, LastName, Email, PasswordHash, Role) " +
                    "VALUES (@FirstName, @LastName, @Email, @PasswordHash, @Role)", connection);

                command.Parameters.AddWithValue("@FirstName", newUser.FirstName);
                command.Parameters.AddWithValue("@LastName", newUser.LastName);
                command.Parameters.AddWithValue("@Email", newUser.Email);
                command.Parameters.AddWithValue("@PasswordHash", newUser.PasswordHash);
                command.Parameters.AddWithValue("@Role", "User");

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }

    }
}
