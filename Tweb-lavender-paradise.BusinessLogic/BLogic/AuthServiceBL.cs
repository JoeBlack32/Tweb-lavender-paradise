using System;
using System.Data.SqlClient;
using System.Linq;
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
                                Role = reader["Role"] != DBNull.Value ? reader["Role"].ToString() : "User",
                                AvatarPath = reader["AvatarPath"] != DBNull.Value ? reader["AvatarPath"].ToString() : "",
                                CartId = reader["CartId"] != DBNull.Value ? reader["CartId"].ToString() : "",
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

        //public UserModel Authenticate(string email, string password)
        //{
        //    using (var db = new UserContext())
        //    {
        //        // Найдём пользователя по email
        //        var userEntity = db.Users.FirstOrDefault(u => u.Email == email);

        //        if (userEntity != null)
        //        {
        //            string storedHash = userEntity.Password ?? "";

        //            if (VerifyPassword(password, storedHash))
        //            {
        //                return new UserModel
        //                {
        //                    Id = userEntity.Id,
        //                    FirstName = userEntity.FirstName,
        //                    LastName = userEntity.LastName,
        //                    Email = userEntity.Email,
        //                    PasswordHash = storedHash,
        //                    Role = userEntity.Role.ToString() // если Level — это enum URole
        //                };
        //            }
        //        }
        //    }

        //    return null;
        //}


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
        //public UserModel Register(UserModel newUser)
        //{
        //    using (var db = new UserContext())
        //    {
        //        var userEntity = new UserDBTable
        //        {
        //            FirstName = newUser.FirstName,
        //            LastName = newUser.LastName,
        //            Email = newUser.Email,
        //            Password = newUser.PasswordHash, // если это то же самое поле
        //            Role = URole.User,              // Enum User Role
        //            LastLogin = DateTime.Now,
        //            LastIp = "0.0.0.0"
        //        };

        //        db.Users.Add(userEntity);
        //        db.SaveChanges();

        //        // Если SaveChanges() успешно, можно вернуть newUser
        //        return newUser;
        //    }
        //}

    }
}
