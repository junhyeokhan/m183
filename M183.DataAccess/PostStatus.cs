using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.DataAccess
{
    public class PostStatus
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual Post Post { get; set; }
    }
}
