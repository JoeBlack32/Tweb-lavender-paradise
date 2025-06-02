using Tweb_lavender_paradise.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweb_lavender_paradise.BusinessLogic;
using Tweb_lavender_paradise.BusinessLogic.BLogic;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;
using Tweb_lavender_paradise.Domain.Enitities.User;
using Tweb_lavender_paradise.Domain.Models;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;


namespace LavenderParadise.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IProductService _productService;
        public IUser _user;
        public HomeController()
        {
            var bl = new BusinessLogic();
            _user = bl.GetUserBL();
            _productService = new ProductServiceBL();
        }

        private readonly string _connectionString = "Data Source=LocalHost;Initial Catalog=LavenderParadise;Integrated Security=True;MultipleActiveResultSets=True;App=LavenderParadise";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Catalog()
        {
            var middleProducts = _productService.GetAllProducts();

            var viewProducts = new List<ProductView>();
            foreach (var p in middleProducts)
            {
                viewProducts.Add(new ProductView
                {
                    GoodCode = p.GoodCode,
                    GoodName = p.GoodName,
                    GoodDescription = p.GoodDescription,
                    GoodPrice = p.GoodPrice,
                    ImgSrc = p.ImgSrc,
                    Category = p.Category
                });
            }

            return View(viewProducts);
        }


        public ActionResult About()
        {
            return View();
        }

 
        public ActionResult Avtorization()
        {
            return View();
        }
        public ActionResult Registration()
        {
            return View();
        }
    }
}