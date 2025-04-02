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

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Храним хеш пароля
        public string Role { get; set; } // "Guest", "User", "Admin"
        public Product SingleProduct { get; set; }
    }
}
