using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                ViewBag.User = null;
            }
        }
    }
}