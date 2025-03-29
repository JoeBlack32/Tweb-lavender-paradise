using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.BusinessLogic.Core.User;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;


namespace Tweb_lavender_paradise.BusinessLogic.BLogic
{
    public class UserBL : UserApi, IUser
    {
        public bool IsValidSession(string key)
        {
            return IsValidSessionAction(key);
        }
    }
}
