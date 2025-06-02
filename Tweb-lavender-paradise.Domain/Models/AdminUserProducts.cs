using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweb_lavender_paradise.Domain.Models
{
    public class AdminUserProducts
    {
        public List<UserModel> Users { get; set; }
        public List<Product> Products { get; set; }
        public List<string> Categories { get; set; }
    }

}
