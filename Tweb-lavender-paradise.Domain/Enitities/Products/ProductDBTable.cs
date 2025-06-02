using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tweb_lavender_paradise.Domain.Models
{
    [Table("Product")]
    public class ProductDBTable
    {
        [Key]
        public int GoodCode { get; set; }

        [Required]
        public string GoodName { get; set; }

        [Required]
        public int Category { get; set; }

        [Required]
        public decimal GoodPrice { get; set; }

        public string GoodDescription { get; set; }

        public string ImgSrc { get; set; }
    }
}
