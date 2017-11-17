using M183.BusinessLogic.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace M183.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static List<BusinessRole> UserRoles;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //TODO: Remove hardcoded code and add it into database
            BusinessRole userRole = new BusinessRole() { Id = 1, Name = "User" };
            BusinessRole adminRole = new BusinessRole() { Id = 2, Name = "Admin" };

            UserRoles = new List<BusinessRole>();
            UserRoles.Add(userRole);
            UserRoles.Add(adminRole);
        }
    }
}
