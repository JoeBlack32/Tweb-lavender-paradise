using System;
using System.Data.SqlClient;
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
                var command = new SqlCommand("SELECT Id, FirstName, LastName, Email, PasswordHash, Role FROM Users WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedHash = reader["PasswordHash"] != DBNull.Value ? reader["PasswordHash"].ToString() : "";

                        if (VerifyPassword(password, storedHash))
                        {
                            var user = new UserModel
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                FirstName = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString() : "",
                                LastName = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString() : "",
                                Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "",
                                PasswordHash = storedHash,
                                Role = reader["Role"] != DBNull.Value ? reader["Role"].ToString() : "User"
                            };
                            return user;
                        }
                    }
                }
            }

            return null;
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // Заменить на bcrypt/argon2 проверку при необходимости
            return password == storedHash;
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
                command.Parameters.AddWithValue("@PasswordHash", newUser.PasswordHash);
                command.Parameters.AddWithValue("@Role", "User");

                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    return newUser;
                }
                else return null;
            }
        }
    }
}
