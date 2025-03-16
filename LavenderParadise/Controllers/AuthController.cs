using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tweb_lavender_paradise.BusinessLogic;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;

namespace LavenderParadise.Controllers
{
    public class AuthController
    {

        private readonly IUser user;
        public AuthController()
        {
            var bl = new BusinessLogic();
        }
    }
}