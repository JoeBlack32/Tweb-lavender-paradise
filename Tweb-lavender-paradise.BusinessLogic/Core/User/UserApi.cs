using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweb_lavender_paradise.BusinessLogic.Core.User
{
    public class UserApi
    {
        public UserApi()
        {

        }

        public bool IsValidSessionAction(string key)
        {
            if (string.IsNullOrEmpty(key)) {
                return false;
            }
            return true;
        }

        public bool IsProductValidAction(int id)
        {
            return true;
        }
    }
}
