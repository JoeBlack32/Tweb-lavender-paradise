using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tweb_lavender_paradise.ViewModels
{
    public class ProductView
    {
        public int GoodCode { get; set; }
        public string GoodName { get; set; }
        public int Category { get; set; }
        public decimal GoodPrice { get; set; }
        public string GoodDescription { get; set; }
        public string ImgSrc { get; set; }
    }
}
