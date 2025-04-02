using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;

namespace LavenderParadise.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IProductService _productService;

        public AdminController(IProductService productService)
        {
            _productService = productService;
        }

        //Нужно добавить эту страницу
        public ActionResult AdminPanel()
        {
            if (Session["UserRole"]?.ToString() != "Admin")
                return RedirectToAction("Index", "Home"); // Перенаправляем, если не админ

            var products = _productService.GetAllProducts();
            return View(products);
        }
    }
}