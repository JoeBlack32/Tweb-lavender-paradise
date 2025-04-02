using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.BusinessLogic.BLogic;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;

namespace Tweb_lavender_paradise.BusinessLogic
{
    public class BusinessLogic
    {
        public IUser GetUserBL()
        {
            return new UserBL();
        }

    }

}
