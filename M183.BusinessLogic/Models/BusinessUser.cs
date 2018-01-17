using System.Collections.Generic;
using System.Web;

namespace M183.BusinessLogic.Models
{
    public class BusinessUser
    {
        private static Repository repository = new Repository();

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

        #region Properties
        public int Id { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsVerified { get; set; }
        public string Username { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public List<Role> Roles { get; set; }
        public AuthenticationMethod AuthenticationMethod { get; set; }
        public bool IsAuthenticated
        {
            get
            {
                return Id > 0 && IsVerified;
            }
        }
        #endregion

        public bool HasRole(Role role)
        {
            return Roles.Contains(role);
        }

        public bool IsAuthorisedToPost(int postId)
        {
            return repository.IsUserAuthorisedToPost(Id, postId);
        }

        public void Logout()
        {
            HttpContext.Current.Session["BusinessUser"] = null;
        }
    }
}
