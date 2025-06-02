using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tweb_lavender_paradise.Domain.Entities
{
    [Table("Cart")]
    public class CartDBTable
    {
        [Key]
        public int Id { get; set; }

        public string Goods { get; set; }
    }
}
