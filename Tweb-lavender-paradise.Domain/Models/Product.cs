using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweb_lavender_paradise.Domain.Models
{
    public class Product
    {
        public int GoodCode { get; set; }
        public string GoodName { get; set; }
        public string Category { get; set; }
        public decimal GoodPrice { get; set; }
        public string GoodDescription { get; set; }
        public string ImgSrc { get; set; }
    }
}
