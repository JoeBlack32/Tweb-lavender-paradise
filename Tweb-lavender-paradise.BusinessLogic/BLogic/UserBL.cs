    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.BusinessLogic.Core.User;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;
using Tweb_lavender_paradise.Domain.Enitities.User;


namespace Tweb_lavender_paradise.BusinessLogic.BLogic
{
    public class UserBL : UserApi, IUser
    {
        public bool IsValidSession(string key)
        {
            return IsValidSessionAction(key);
        }

        public bool AddUser(UserDBTable data)
        {
            AddUserApi(data);

            return true;
        }
    }
}
