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
            var user = HttpContext.Session["User"];
            if (user == null)
                return RedirectToAction("Login");

            return View(user); // передаём данные пользователя
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

    }
}