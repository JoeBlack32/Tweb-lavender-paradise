using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

                var command = new SqlCommand("SELECT Id, FirstName, LastName, Email, PasswordHash, Role FROM Users", connection);

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
                        };

                        users.Add(user1);
                    }
                }
            }

            return View(users);
        }


    }
}