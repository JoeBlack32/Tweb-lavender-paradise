using LavenderParadise.Controllers;
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
    public class ProductController : BaseController
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
                    return View(product);
                }
            }
            return RedirectToAction("Index", "Home"); // Если товара нет, уходим на главную
        }

        [HttpPost]
        public ActionResult AddToCart(int productId)
        {
            if (ViewBag.User != null && ViewBag.User.Id > 0)
            {
                var userId = ViewBag.User.Id;
                _productService.AddProductToCart(userId, productId);
            }

            return RedirectToAction("Catalog", "Home");
        }

        public ActionResult Cart()
        {
            var user = ViewBag.User as UserModel;
            if (user == null || user.Id == 0)
                return RedirectToAction("Index", "Home");

            var cartProducts = _productService.GetCartProductsByUserId(user.Id);

            ViewBag.User = user;
            return View(cartProducts);
        }

        [HttpPost]
        public ActionResult Increase(int productId)
        {
            var user = ViewBag.User as UserModel;
            if (user == null || string.IsNullOrWhiteSpace(user.CartId))
                return RedirectToAction("Index", "Home");

            if (int.TryParse(user.CartId, out int cartId))
            {
                _productService.IncreaseProductQuantityInCart(cartId, productId);
            }

            return RedirectToAction("Cart");
        }

        [HttpPost]
        public ActionResult Decrease(int productId)
        {
            var user = ViewBag.User as UserModel;
            if (user == null || string.IsNullOrWhiteSpace(user.CartId))
                return RedirectToAction("Index", "Home");

            if (int.TryParse(user.CartId, out int cartId))
            {
                _productService.DecreaseProductQuantityInCart(cartId, productId);
            }

            return RedirectToAction("Cart");
        }

        [HttpPost]
        public ActionResult Remove(int productId)
        {
            var user = ViewBag.User as UserModel;
            if (user == null || string.IsNullOrWhiteSpace(user.CartId))
                return RedirectToAction("Index", "Home");

            if (int.TryParse(user.CartId, out int cartId))
            {
                _productService.DeleteProductFromCart(cartId, productId);
            }

            return RedirectToAction("Cart");
        }

        [HttpPost]
        public ActionResult ConfirmOrder()
        {
            var user = ViewBag.User as UserModel;
            if (user == null)
                return RedirectToAction("Login", "Account");

            string error;
            bool success = _productService.ConfirmOrder(user, out error);

            if (!success)
            {
                TempData["OrderError"] = error;
                return RedirectToAction("Cart");
            }

            return RedirectToAction("PersonalAccount", "Account");
        }


    }
}