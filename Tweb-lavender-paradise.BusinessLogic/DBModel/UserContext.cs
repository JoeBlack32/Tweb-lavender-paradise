using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweb_lavender_paradise.BusinessLogic.DBModel
{
    public class UserContext : DbContext
    {
        public UserContext() :
            base("name=TwebLavenderParadise") // connectionstring name define in your web.config
        {
        }

        public virtual DbSet<UserDBTable> Users { get; set; }
    }
}
