﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweb_lavender_paradise.BusinessLogic.BLogic;
using Tweb_lavender_paradise.BusinessLogic.DBModel;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;
using Tweb_lavender_paradise.Domain.Models;

namespace LavenderParadise.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IUser _UserService = new UserBL();

        private readonly string _connectionString = "Data Source=LocalHost;Initial Catalog=LavenderParadise;Integrated Security=True;MultipleActiveResultSets=True;App=LavenderParadise";

        [HttpGet]
        public ActionResult AdminPanel()
        {
            var sessionUser = (UserModel)HttpContext.Session["User"];
            if (sessionUser == null || sessionUser.Role != "Admin")
                return RedirectToAction("Login");

            var users = new List<UserModel>();
            var products = new List<Product>();

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
                            CartId = reader["CartId"].ToString()
                        };
                        users.Add(user);
                    }
                }

                // Загрузка товаров
                var productCmd = new SqlCommand("SELECT * FROM Product", connection);
                using (var reader = productCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = new Product
                        {
                            GoodCode = Convert.ToInt32(reader["GoodCode"]),
                            GoodName = reader["GoodName"].ToString(),
                            GoodDescription = reader["GoodDescription"].ToString(),
                            GoodPrice = Convert.ToDecimal(reader["GoodPrice"]),
                            ImgSrc = reader["ImgSrc"].ToString()
                        };
                        products.Add(product);
                    }
                }
            }

            var model = new AdminUserProducts
            {
                Users = users,
                Products = products
            };

            return View(model);
        }

        [HttpPost]
                public ActionResult AddProduct(string GoodName, string Category, decimal GoodPrice, string GoodDescription, HttpPostedFileBase ImgSrc)
                {
                    string fileName = null;

                    if (ImgSrc != null && ImgSrc.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(ImgSrc.FileName);
                        string path = Path.Combine(Server.MapPath("~/src/ProductImages"), fileName);
                        Directory.CreateDirectory(Server.MapPath("~/src/ProductImages")); // на всякий случай
                        ImgSrc.SaveAs(path);
                    }

                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        var command = new SqlCommand(@"
                        INSERT INTO Product (GoodName, Category, GoodPrice, GoodDescription, ImgSrc)
                        VALUES (@name, @cat, @price, @desc, @img)", connection);

                        command.Parameters.AddWithValue("@name", GoodName);
                        command.Parameters.AddWithValue("@cat", Category);
                        command.Parameters.AddWithValue("@price", GoodPrice);
                        command.Parameters.AddWithValue("@desc", GoodDescription);
                        command.Parameters.AddWithValue("@img", fileName ?? "");

                        command.ExecuteNonQuery();
                    }

                    return RedirectToAction("AdminPanel");
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

            [HttpPost]
            public ActionResult UpdateBalance(int userId, decimal newBalance)
            {
                _UserService.UpdateUserBalance(userId, newBalance); // или _productService
                return RedirectToAction("AdminPanel");
            }

    }


}
