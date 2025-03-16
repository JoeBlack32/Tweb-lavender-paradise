using LavenderParadise.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweb_lavender_paradise.BusinessLogic;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;

namespace LavenderParadise.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProduct _product;
        public ProductController()
        {
            var bl = new BusinessLogic();
            _product = bl.GetProductBL();
        }

        public ActionResult ProductDetails()
        {
            var id = Request.QueryString["product"];
            var lID = Convert.ToUInt16(id);

            if (!_product.IsProductValid(lID))
            {
                return View();
            }
            return View();

        }
    }
}