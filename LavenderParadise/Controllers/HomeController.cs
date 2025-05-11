using LavenderParadise.Models;
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
        public IUser _user;
        public HomeController()
        {
            var bl = new BusinessLogic();
            _user = bl.GetUserBL();

        }

        private readonly IProductService _productService;
        private readonly string _connectionString = "Data Source=LocalHost;Initial Catalog=LavenderParadise;Integrated Security=True;MultipleActiveResultSets=True;App=LavenderParadise";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Catalog()
        {
            var goods = new List<Product>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT GoodName, GoodCode, GoodDescription, GoodPrice, ImgSrc, Category FROM Product", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = new Product
                        {
                            GoodName = reader["GoodName"]?.ToString(),
                            GoodCode = reader["GoodCode"] != DBNull.Value ? Convert.ToInt32(reader["GoodCode"]) : 0,
                            GoodDescription = reader["GoodDescription"]?.ToString(),
                            GoodPrice = reader["GoodPrice"] != DBNull.Value ? Convert.ToDecimal(reader["GoodPrice"]) : 0,
                            ImgSrc = reader["ImgSrc"]?.ToString(),
                            Category = reader["Category"]?.ToString()
                        };

                        goods.Add(product);
                    }
                }
            }

            return View(goods);
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