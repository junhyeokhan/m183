using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.DataAccess
{
    public class Token
    {
        public int Id { get; set; }

        public string TokenString { get; set; }
        public DateTime Expiry { get; set; }
        public bool Deleted { get; set; }

        public virtual User User { get; set; }
    }
}
