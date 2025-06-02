using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweb_lavender_paradise.BusinessLogic;
using Tweb_lavender_paradise.Domain.Models;

namespace LavenderParadise.Controllers
{
    public class CartController : BaseController
    {
        private readonly CartServiceBL _cartService;

        public CartController()
        {
            _cartService = new CartServiceBL();
        }

        [HttpPost]
        public ActionResult AddToCart(int productId)
        {
            UserModel user = ViewBag.User as UserModel;
            if (user != null && user.Id > 0)
            {
                _cartService.AddProductToCart(user.Id, productId);
            }

            return RedirectToAction("Catalog", "Home");
        }

        public ActionResult Cart()
        {
            var user = ViewBag.User as UserModel;
            if (user == null || user.Id == 0)
                return RedirectToAction("Index", "Home");

            var cartProducts = _cartService.GetCartProductsByUserId(user.Id);

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
                _cartService.IncreaseQuantity(cartId, productId);
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
                _cartService.DecreaseQuantity(cartId, productId);
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
                _cartService.DeleteFromCart(cartId, productId);
            }

            return RedirectToAction("Cart");
        }

        [HttpPost]
        public ActionResult ConfirmOrder()
        {
            var user = ViewBag.User as UserModel;
            if (user == null)
                return RedirectToAction("Login", "Account");

            string errorMessage;
            bool success = _cartService.ConfirmOrder(user, out errorMessage);

            if (!success)
            {
                TempData["OrderError"] = errorMessage;
                return RedirectToAction("Cart");
            }

            return RedirectToAction("PersonalAccount", "Home");
        }
    }
}