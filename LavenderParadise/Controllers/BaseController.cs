using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LavenderParadise.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userRole = Session["UserRole"] as string ?? "Guest"; // Если нет сессии, то гость
            ViewBag.UserRole = userRole;
            base.OnActionExecuting(filterContext);
        }
    }
}