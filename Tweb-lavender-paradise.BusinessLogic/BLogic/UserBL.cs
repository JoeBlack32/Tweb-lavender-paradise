    using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.BusinessLogic.Core.User;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;
using Tweb_lavender_paradise.Domain.Enitities.User;
using Tweb_lavender_paradise.Domain.Models;


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

        public UserModel UpdateUserBalance(int userId, decimal newBalance, UserModel user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand("UPDATE Users SET Balance = @Balance WHERE Id = @UserId", connection);
                cmd.Parameters.AddWithValue("@Balance", newBalance);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.ExecuteNonQuery();
                user.Balance = newBalance;
                return user;
            }
        }

        public string HashPassword(string password)
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

        public bool VerifyPassword(string password, string storedHash)
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

        public void UpdatePassword(int userId, string newPassword)
        {
            string hash = HashPassword(newPassword);

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE Users SET PasswordHash=@PasswordHash WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", userId);
                command.Parameters.AddWithValue("@PasswordHash", hash);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public string GetPasswordHashByUserId(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT PasswordHash FROM Users WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", userId);

                connection.Open();
                var result = command.ExecuteScalar();
                return result != null ? result.ToString() : null;
            }
        }

        public List<UserModel> GetAllUsers()
        {
            List<UserModel> users = new List<UserModel>();
            using (var connection = new SqlConnection(_connectionString))
                {
                connection.Open();
                    // Загрузка пользователей
                    var userCmd = new SqlCommand("SELECT * FROM Users", connection);
                    using (var reader = userCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new UserModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString(),
                                Role = reader["Role"].ToString(),
                                AvatarPath = reader["AvatarPath"].ToString(),
                                CartId = reader["CartId"].ToString(),
                                Balance = Convert.ToInt32(reader["Balance"])
                            };
                        users.Add(user);
                        }
                    }
                }
            return users;
        }
    }
}
