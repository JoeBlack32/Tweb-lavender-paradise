using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;
using Tweb_lavender_paradise.Domain.Models;

namespace Tweb_lavender_paradise.BusinessLogic.BLogic
{
    public class ProductService : IProductService
    {
        private static readonly List<Product> products = new List<Product>
        {
            new Product { GoodCode = 1, GoodName = "Масло лаванды", Category = "Aromatherapy", GoodPrice = 500, GoodDescription = "Натуральное масло", ImgSrc = "/images/lavender_oil.jpg" },
            new Product { GoodCode = 2, GoodName = "Лавандовый чай", Category = "Aromatherapy", GoodPrice = 400, GoodDescription = "Лавандовый чай", ImgSrc = "/images/mint_oil.jpg" },
            new Product { GoodCode = 3, GoodName = "Мыло с лавандой", Category = "Aromatherapy", GoodPrice = 400, GoodDescription = "Мыло с лавандой", ImgSrc = "/images/mint_oil.jpg" }
        };

        public Product GetProductById(int goodCode)
        {
            foreach(var product in products)
            {
                if(product.GoodCode == goodCode)
                {
                    return product;
                }
            }
            return null;
        }

        public List<Product> GetAllProducts()
        {
            return products;
        }
    }
}
