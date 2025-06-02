using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.Domain.Entities;
using Tweb_lavender_paradise.Domain.Models;


namespace Tweb_lavender_paradise.BusinessLogic.DBModel
{
    public class ProductContext : DbContext
    {
        public ProductContext() :
            base("name=LavenderParadise") // Использует ту же строку подключения
        {
        }

        public virtual DbSet<ProductDBTable> Products { get; set; }
        public virtual DbSet<CategoryDBTable> Categories { get; set; }
    }
}
