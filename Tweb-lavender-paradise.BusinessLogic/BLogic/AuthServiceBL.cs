using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Tweb_lavender_paradise.BusinessLogic.DBModel;
using Tweb_lavender_paradise.Domain.Enitities.User;
using Tweb_lavender_paradise.Domain.Models;

namespace Tweb_lavender_paradise.BusinessLogic.BLogic
{
    public class AuthServiceBL
    {
        private readonly string _connectionString = "Data Source=LocalHost;Initial Catalog=LavenderParadise;Integrated Security=True;MultipleActiveResultSets=True;App=LavenderParadise";

        public UserModel Authenticate(string email, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedHash = reader["PasswordHash"]?.ToString();

                        if (VerifyPassword(password, storedHash))
                        {
                            var user = new UserModel
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                FirstName = reader["FirstName"]?.ToString() ?? "",
                                LastName = reader["LastName"]?.ToString() ?? "",
                                Email = reader["Email"]?.ToString() ?? "",
                                PasswordHash = storedHash,
                                Role = reader["Role"]?.ToString() ?? "User",
                                AvatarPath = reader["AvatarPath"]?.ToString() ?? "",
                                CartId = reader["CartId"]?.ToString() ?? "",
                                OrderHistoryId = reader["OrderHistoryId"] != DBNull.Value ? Convert.ToInt32(reader["OrderHistoryId"]) : 0,
                                Balance = reader["Balance"] != DBNull.Value ? Convert.ToDecimal(reader["Balance"]) : 0,
                            };
                            return user;
                        }
                    }
                }
            }

            return null;
        }


        private string HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt);

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);

                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                return Convert.ToBase64String(hashBytes);
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }

            return true;
        }


        public UserModel Register(UserModel newUser)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Users (FirstName, LastName, Email, PasswordHash, Role) " +
                    "VALUES (@FirstName, @LastName, @Email, @PasswordHash, @Role)", connection);

                command.Parameters.AddWithValue("@FirstName", newUser.FirstName);
                command.Parameters.AddWithValue("@LastName", newUser.LastName);
                command.Parameters.AddWithValue("@Email", newUser.Email);

                string hashedPassword = HashPassword(newUser.PasswordHash); // Хешируем пароль
                command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                command.Parameters.AddWithValue("@Role", "User");

                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    newUser.PasswordHash = hashedPassword;
                    return newUser;
                }
                else return null;
            }
        }

    }
}
