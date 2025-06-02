using System.Collections.Generic;
using System.Linq;
using Tweb_lavender_paradise.BusinessLogic.DBModel;
using Tweb_lavender_paradise.Domain.Entities;
using Tweb_lavender_paradise.Domain.Models; // ProductDBTable

namespace Tweb_lavender_paradise.BusinessLogic.Core.Product
{
    public class ProductApi
    {
        public List<ProductDBTable> GetAllProductsApi()
        {
            using (var db = new ProductContext())
            {
                return db.Products.ToList(); // возвращаем напрямую DB-модели
            }
        }

        public ProductDBTable GetProductByIdApi(int id)
        {
            using (var db = new ProductContext())
            {
                return db.Products.FirstOrDefault(x => x.GoodCode == id);
            }
        }

        public static List<CategoryDBTable> GetAllCategories()
        {
            using (var db = new ProductContext())
            {
                return db.Categories.ToList();
            }
        }
    }
}
