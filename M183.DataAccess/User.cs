using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.DataAccess
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        //TODO: Save username as hash
        public string UserName { get; set; }
        //TODO: Save password as hash
        public string Password { get; set; }
        public int AuthenticationMode { get; set; }

        public virtual ICollection<Token> Tokens { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserStatus> UserStatues { get; set; }
    }
}
