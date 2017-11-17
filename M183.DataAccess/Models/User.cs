using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.DataAccess.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<PersonalDocument> PersonalDocuments { get; set; }
    }
}
