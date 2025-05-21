using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.Domain.Enitities.User;
using Tweb_lavender_paradise.Domain.Models;

namespace Tweb_lavender_paradise.BusinessLogic.Interfaces
{
    public interface IUser
    {
        bool IsValidSession(string key);
        bool AddUser(UserDBTable data);
        void UpdatePassword(int userId, string newPassword);
        UserModel UpdateUserBalance(int userId, decimal newBalance, Domain.Models.UserModel u);
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedHash);
        string GetPasswordHashByUserId(int userId);
        List<UserModel> GetAllUsers();
    }
}
