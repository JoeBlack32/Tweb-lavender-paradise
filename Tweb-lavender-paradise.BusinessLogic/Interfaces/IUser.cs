using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.Domain.Enitities.User;

namespace Tweb_lavender_paradise.BusinessLogic.Interfaces
{
    public interface IUser
    {
        bool IsValidSession(string key);
        bool AddUser(UserDBTable data);
        void UpdatePassword(int userId, string newPassword);
    }
}
