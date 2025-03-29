using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweb_lavender_paradise.Domain.Models
{
    public class UserModel
    {
        public string btn;

        public string Name { get; set; }
        public string Role { get; set; }
        public Product SingleProduct { get; set; }
    }
}
