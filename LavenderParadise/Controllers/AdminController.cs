using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Tweb_lavender_paradise.BusinessLogic.BLogic;
using Tweb_lavender_paradise.BusinessLogic.DBModel;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;
using Tweb_lavender_paradise.Domain.Models;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
//using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace LavenderParadise.Controllers
{
    //[AdminOnly]
    public class AdminController : BaseController
    {
        private readonly IProductService _productService = new ProductServiceBL();
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

            var model = new AdminUserProducts
            {
                Users = _UserService.GetAllUsers(),
                Products = _productService.GetAllProducts(),
                //Categories = _productService.GetAllCategories() // получаем категории
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
                public ActionResult UpdatePassword(int? id, string newPassword)
                {
                    if (id == null)
                        return RedirectToAction("AdminPanel");

                    _UserService.UpdatePassword(id.Value, newPassword);

                    return RedirectToAction("AdminPanel");
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

                UserModel u = Session["User"] as UserModel;
                u = _UserService.UpdateUserBalance(userId, newBalance, u); // или _productService
                Session["User"] = u;
                return RedirectToAction("AdminPanel");

            }
    }


}
