using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.DataAccess.Models
{
    public class Role
    {
        public int Id { get; set; }

        public int RoleType { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
