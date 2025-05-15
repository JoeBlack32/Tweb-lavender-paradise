using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweb_lavender_paradise.Domain.Models;
using Tweb_lavender_paradise.BusinessLogic.BLogic;


namespace LavenderParadise.Controllers
{
    public class AccountController : BaseController
    {
        private readonly AuthServiceBL _authService = new AuthServiceBL();
        private readonly UserBL _userService = new UserBL();

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
            return View("Avtorization"); // Перерисовываем страницу с ошибкой
        }

        [HttpGet]
        public ActionResult PersonalAccount()
        {
            var user = HttpContext.Session["User"] as UserModel;
            if (user == null)
                return RedirectToAction("Login");

            var productService = new ProductService();
            var orders = productService.GetOrdersByUserId(user.Id); // Список (List<Product>, decimal)

            ViewBag.OrderHistory = orders;
            return View(user); // передаём данные пользователя в сам View
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
            var user = ViewBag.User as UserModel; // расширение или ручной десериализатор

            if (user == null)
            {
                TempData["PasswordError"] = "Пользователь не авторизован.";
                return RedirectToAction("PersonalAccount", "Account");
            }

            // Проверь старый пароль
            if (user.PasswordHash != oldPassword)
            {
                TempData["PasswordError"] = "Старый пароль введен неверно.";
                return RedirectToAction("PersonalAccount", "Account");
            }

            if (newPassword != confirmPassword)
            {
                TempData["PasswordError"] = "Новый пароль и его подтверждение не совпадают.";
                return RedirectToAction("PersonalAccount", "Account");
            }

            // Обновляем пароль в базе
            _userService.UpdatePassword(user.Id, newPassword);

            // Обновляем сессию
            user.PasswordHash = newPassword;
            Session["User"] = user;

            TempData["PasswordSuccess"] = "Пароль успешно изменён.";
            return RedirectToAction("PersonalAccount", "Account");
        }


    }
}