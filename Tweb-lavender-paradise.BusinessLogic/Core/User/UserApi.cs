using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.BusinessLogic.DBModel;
using Tweb_lavender_paradise.Domain.Enitities.User;

namespace Tweb_lavender_paradise.BusinessLogic.Core.User
{
    public class UserApi
    {
        public void AddUserApi(UserDBTable data)
        {
            data.LastLogin = DateTime.Now;
            data.LastIp = "";
            data.Password = "123456789";
            data.Level = 0;
            using (var db = new UserContext()) {
            db.Users.Add(data);
                db.SaveChanges();
            }

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
