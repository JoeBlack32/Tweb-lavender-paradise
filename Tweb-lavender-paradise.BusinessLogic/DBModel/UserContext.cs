using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.Domain.Entities;
using Tweb_lavender_paradise.Domain.Enitities.User;

namespace Tweb_lavender_paradise.BusinessLogic.DBModel
{
    public class UserContext : DbContext
    {
        public UserContext() :
            base("name=LavenderParadise") // connectionstring name define in your web.config
        {

        }

        public virtual DbSet<UserDBTable> Users { get; set; }
        public virtual DbSet<OrderHistoryDBTable> OrderHistory { get; set; }
    }
}
