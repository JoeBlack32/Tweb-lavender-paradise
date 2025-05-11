using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Tweb_lavender_paradise.Domain.Models;

namespace LavenderParadise.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (Session["User"] is UserModel user)
            {
                ViewBag.User = user;
            }
            else
            {
                var u = new UserModel
                {
                    FirstName = "",
                    LastName = "",
                    Email = "",
                    PasswordHash = "",
                    Role = "Guest",
                };
                Session["User"] = u;
                ViewBag.User = null;
            }
        }
    }
}