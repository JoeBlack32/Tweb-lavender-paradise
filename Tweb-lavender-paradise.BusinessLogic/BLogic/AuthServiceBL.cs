using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.Domain.Models;

namespace Tweb_lavender_paradise.BusinessLogic.BLogic
{
    public class AuthServiceBL
    {
        private readonly List<UserModel> _users = new List<UserModel>
        {
            new UserModel { Id = 1, FirstName = "Admin", LastName = "Slava", Email = "admin@example.com", PasswordHash = "hashed_password", Role = "Admin" },
            new UserModel { Id = 2, FirstName = "User", LastName = "Slava", Email = "user@example.com", PasswordHash = "hashed_password", Role = "User" }
        };

        public UserModel Authenticate(string email, string password)
        {
            var user = _users.FirstOrDefault(u => u.Email == email);
            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                return user;
            }
            return null;
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // Логика проверки пароля (bcrypt или другой хеш)
            return password == storedHash;
        }

        public bool Register(UserModel newUser)
        {
            if (_users.Any(u => u.Email == newUser.Email))
                return false; // Email уже используется

            newUser.Id = _users.Count + 1;
            newUser.Role = "User"; // По умолчанию обычный пользователь
            _users.Add(newUser);
            return true;
        }
    }
}
