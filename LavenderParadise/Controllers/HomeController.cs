using LavenderParadise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LavenderParadise.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Catalog()
        {
            UserData u = new UserData();
            u.Username = "Guest";
            u.Products = new List<string> { "Product #1", "Product #2", "Product #3", "Product #4" };
            return View(u);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }

        public ActionResult Avtorization()
        {
            return View();
        }
    }
}