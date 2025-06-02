using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweb_lavender_paradise.Domain.Models;
using Tweb_lavender_paradise.BusinessLogic.BLogic;
using System.Data.SqlClient;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;
using System.Security.Cryptography;
using Microsoft.Ajax.Utilities;
using Tweb_lavender_paradise.BusinessLogic;


namespace LavenderParadise.Controllers
{
    public class AccountController : BaseController
    {

        private readonly AuthServiceBL _authService = new AuthServiceBL();
        private readonly UserBL _userService = new UserBL();
        private readonly ProductServiceBL _productService = new ProductServiceBL();
        private readonly string _connectionString = "Data Source=LocalHost;Initial Catalog=LavenderParadise;Integrated Security=True;MultipleActiveResultSets=True;App=LavenderParadise";

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = _authService.Authenticate(email, password);
            if (user != null)
            {
                Session["User"] = user;

                if (user.Role.ToLower() == "admin")
                    return RedirectToAction("AdminPanel", "Admin");

                return RedirectToAction("PersonalAccount", "Account");
            }

            ViewBag.ErrorMessage = "Неверный email или пароль.";
            return RedirectToAction("Avtorization", "Home"); // Перерисовываем страницу с ошибкой
        }

        [HttpGet]
        public ActionResult PersonalAccount()
        {
            var sessionUser = (UserModel)Session["User"];
            if (sessionUser == null)
                return RedirectToAction("Login");

            // Обновление данных пользователя из базы
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", sessionUser.Id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var freshUser = new UserModel
                        {
                            Id = (int)reader["Id"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Role = reader["Role"].ToString(),
                            Balance = Convert.ToDecimal(reader["Balance"]),
                            CartId = reader["CartId"]?.ToString(),
                            AvatarPath = reader["AvatarPath"]?.ToString(),
                            OrderHistoryId = (int)reader["Id"]
                        };

                        Session["User"] = freshUser;
                        sessionUser = freshUser;
                    }
                }
            }

            //// Получаем актуальные заказы
            //var orders = _productService.GetOrdersByUserId(sessionUser.Id);
            //ViewBag.OrderHistory = orders;

            return View(sessionUser);
        }


        //[HttpPost]
        //public ActionResult PersonalAccount()
        //{
        //    return View(); // Вернёт Views/Account/PersonalAccount.cshtml
        //}

        [HttpPost]
        public ActionResult Register(string firstName, string lastName, string email, string password)
        {
            var newUser = new UserModel
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = password
            };
            newUser = _authService.Register(newUser);
            if (newUser != null)
            {
                Session["User"] = newUser;
                return RedirectToAction("PersonalAccount", "Account");
            }

            return Json(new { success = false, message = "Пользователь с таким email уже зарегистрирован." });
        }

        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            var user = ViewBag.User as UserModel;

            if (user == null)
            {
                TempData["PasswordError"] = "Пользователь не авторизован.";
                return RedirectToAction("PersonalAccount", "Account");
            }

            // Проверяем старый пароль с помощью UserBL
            if (!_userService.VerifyPassword(oldPassword, user.PasswordHash))
            {
                TempData["PasswordError"] = "Старый пароль введен неверно.";
                return RedirectToAction("PersonalAccount", "Account");
            }

            if (newPassword != confirmPassword)
            {
                TempData["PasswordError"] = "Новый пароль и его подтверждение не совпадают.";
                return RedirectToAction("PersonalAccount", "Account");
            }

            // Обновляем пароль в базе (захешируется внутри)
            _userService.UpdatePassword(user.Id, newPassword);

            // Обновляем сессию (новый хеш)
            user.PasswordHash = _userService.HashPassword(newPassword);
            Session["User"] = user;

            TempData["PasswordSuccess"] = "Пароль успешно изменён.";
            return RedirectToAction("PersonalAccount", "Account");
        }

    }
}