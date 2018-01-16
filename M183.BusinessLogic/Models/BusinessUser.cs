using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace M183.BusinessLogic.Models
{
    public class BusinessUser
    {
        private BusinessUser()
        {

        }
        
        public static BusinessUser Current
        {
            get
            {
                BusinessUser session = (BusinessUser)HttpContext.Current.Session["BusinessUser"];
                if (session == null)
                {
                    session = new BusinessUser();
                    HttpContext.Current.Session["BusinessUser"] = session;
                }
                return session;
            }
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string MobileNumber { get; set; }
    }
}
