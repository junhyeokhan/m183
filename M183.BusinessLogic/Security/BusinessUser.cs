using System;
using System.Web;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace M183.BusinessLogic.Security
{
    public class BusinessUser
    {
        private BusinessUser()
        {
            Id = 0;
            Email = "";
            Roles = new List<BusinessRole>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public List<BusinessRole> Roles { get; set; }

        public static BusinessUser Current
        {
            get
            {
                var user = HttpContext.Current.Session["BusinessUser"] as BusinessUser;
                if (user == null)
                {
                    user = new BusinessUser();
                    HttpContext.Current.Session["BusinessUser"] = user;
                }
                return user;
            }
        }
    }
}
