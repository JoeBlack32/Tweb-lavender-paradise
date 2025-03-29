using LavenderParadise.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweb_lavender_paradise.BusinessLogic;
using Tweb_lavender_paradise.BusinessLogic.BLogic;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;
using Tweb_lavender_paradise.Domain.Models;

namespace Tweb_lavender_paradise.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController()
        {
            _productService = new ProductService();
        }

        [HttpPost]
        public ActionResult ProductDetails(string btn)
        {
            if (int.TryParse(btn, out int id))
            {
                var product = _productService.GetProductById(id);
                if (product != null)
                {
                    var user = new UserModel
                    {
                        Name = "Гость",
                        Role = "User",
                        SingleProduct = product,
                        btn = btn
                    };
                    return View(user);
                }
            }
            return RedirectToAction("Index", "Home"); // Если товара нет, уходим на главную
        }

        //    [HttpPost]
        //    public ActionResult ProductDetails(string btn)
        //    {
        //        return RedirectToAction("ProductDetails", "Product", new { p = btn });
        //    }
    }
}