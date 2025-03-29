using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweb_lavender_paradise.BusinessLogic;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;

namespace LavenderParadise.Controllers
{
    public class AuthController : Controller
    {

        private readonly IUser _user;
        public AuthController()
        {
            var bl = new BusinessLogic();
            _user = bl.GetUserBL();

        }
        public ActionResult Registration()
        {
            string key = "myKey";
            bool isValid = _user.IsValidSession(key);
            return View();
        }
    }
}