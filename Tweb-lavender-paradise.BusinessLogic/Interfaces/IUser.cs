﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweb_lavender_paradise.BusinessLogic.Interfaces
{
    public interface IUser
    {
        bool IsValidSession(string key);
    }
}
