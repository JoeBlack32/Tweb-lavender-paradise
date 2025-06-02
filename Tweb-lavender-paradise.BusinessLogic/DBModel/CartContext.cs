using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.Domain.Entities;
using Tweb_lavender_paradise.BusinessLogic.DBModel;

namespace Tweb_lavender_paradise.BusinessLogic.Context
{
    public class CartContext : DbContext
    {
        public CartContext() : base("name=LavenderParadise") { }

        public virtual DbSet<CartDBTable> Carts { get; set; }
    }
}
