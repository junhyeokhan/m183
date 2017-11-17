using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.DataAccess.Models
{
    public class PersonalDocument : Document
    {
        public User User { get; set; }
    }
}
