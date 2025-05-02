using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweb_lavender_paradise.Domain.Models;
using Tweb_lavender_paradise.BusinessLogic.BLogic;


namespace LavenderParadise.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthServiceBL _authService = new AuthServiceBL();

        [HttpPost]
        public JsonResult Login(string email, string password)
        {
            var user = _authService.Authenticate(email, password);
            if (user != null)
            {
                Session["UserId"] = user.Id;
                Session["UserRole"] = user.Role;

                return Json(new { success = true, redirectUrl = Url.Action("PersonalAccount", "Account") });
            }

            return Json(new { success = false, message = "Неверный email или пароль." });
        }

        [HttpPost]
        public ActionResult PersonalAccount()
        {
            return View(); // Вернёт Views/Account/PersonalAccount.cshtml
        }

        [HttpPost]
        public JsonResult Register(string firstName, string lastName, string email, string password)
        {
            var newUser = new UserModel
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = password
            };

            if (_authService.Register(newUser))
            {
                return Json(new { success = true, redirectUrl = Url.Action("Login") });
            }

            return Json(new { success = false, message = "Пользователь с таким email уже зарегистрирован." });
        }

    }
}