using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweb_lavender_paradise.Domain.Entities
{
    public class OrderHistoryDBTable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Goods { get; set; }
        public decimal CheckOut { get; set; }
    }
}
