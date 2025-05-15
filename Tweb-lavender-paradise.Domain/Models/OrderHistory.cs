using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweb_lavender_paradise.Domain.Models
{
    public class OrderHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Goods { get; set; } // Формат: {"1":2,"3":1}
        public decimal CheckOut { get; set; }
    }
}
