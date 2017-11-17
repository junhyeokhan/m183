using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.DataAccess.Models
{
    public class Permission
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
