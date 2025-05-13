using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweb_lavender_paradise.BusinessLogic.DBModel;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;
using Tweb_lavender_paradise.Domain.Models;

namespace LavenderParadise.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IProductService _productService;
        private readonly string _connectionString = "Data Source=LocalHost;Initial Catalog=LavenderParadise;Integrated Security=True;MultipleActiveResultSets=True;App=LavenderParadise";

        [HttpGet]
        public ActionResult AdminPanel()
        {
            var user = (UserModel)HttpContext.Session["User"];
            if (user == null || user.Role != "Admin")
                return RedirectToAction("Login");

            var users = new List<UserModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT Id, FirstName, LastName, Email, PasswordHash, AvatarPath, Role FROM Users", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user1 = new UserModel
                        {
                            Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                            FirstName = reader["FirstName"]?.ToString(),
                            LastName = reader["LastName"]?.ToString(),
                            Email = reader["Email"]?.ToString(),
                            PasswordHash = reader["PasswordHash"]?.ToString(),
                            Role = reader["Role"]?.ToString(),
                            AvatarPath = reader["AvatarPath"]?.ToString()
                        };

                        users.Add(user1);
                    }
                }
            }

            return View(users);
        }

        [HttpPost]
        public ActionResult UpdateUser(int id, string firstName, string lastName, string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE Users SET FirstName=@FirstName, LastName=@LastName, Email=@Email WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();
                command.ExecuteNonQuery();
            }
            return RedirectToAction("AdminPanel");
        }
        [HttpPost]
        public ActionResult UpdatePassword(int id, string newPassword)
        {
            string hash = HashPassword(newPassword); // реализуйте хеширование
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE Users SET PasswordHash=@PasswordHash WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@PasswordHash", hash);

                connection.Open();
                command.ExecuteNonQuery();
            }
            return RedirectToAction("AdminPanel");
        }

        private string HashPassword(string newPassword)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult UpdateAvatar(int id, HttpPostedFileBase avatar)
        {
            if (avatar != null && avatar.ContentLength > 0)
            {
                string fileName = Path.GetFileName(avatar.FileName);
                string path = Path.Combine(Server.MapPath("../src/Avatars"), fileName);
                avatar.SaveAs(path);

                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand("UPDATE Users SET AvatarPath=@Path WHERE Id=@Id", connection);
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Path", fileName);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("AdminPanel");
        }
        [HttpPost]
        public ActionResult UpdateRole(int id, string newRole)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE Users SET Role=@Role WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Role", newRole);
                connection.Open();
                command.ExecuteNonQuery();
            }
            return RedirectToAction("AdminPanel");
        }
        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM Users WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
            return RedirectToAction("AdminPanel");
        }

    }
}