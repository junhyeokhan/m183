using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.DataAccess
{
    public class UserRole
    {
        public int Id { get; set; }

        public int Role { get; set; }

        public virtual User User { get; set; }
    }
}
