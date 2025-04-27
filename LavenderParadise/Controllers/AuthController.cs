using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweb_lavender_paradise.BusinessLogic;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;
using Tweb_lavender_paradise.BusinessLogic.BLogic;
using Tweb_lavender_paradise.Domain.Enitities.User;


namespace LavenderParadise.Controllers
{
    public class AuthController : Controller
    {

        public  IUser _user;
        public AuthController()
        {
            var bl = new BusinessLogic();
            _user = bl.GetUserBL();

        }
       
    }
}