using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.BusinessLogic.Core.User;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;

namespace Tweb_lavender_paradise.BusinessLogic.BLogic
{
    public class ProductBL : UserApi, IProduct
    {
        public bool IsProductValid(int id)
        {
            return IsProductValidAction(id);
        }
    }
}
