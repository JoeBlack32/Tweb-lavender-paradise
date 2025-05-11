using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweb_lavender_paradise.Domain.Models
{
    public class CatalogModel
    {
        public UserModel User { get; set; }
        public List<Product> Products { get; set; }
    }
}
