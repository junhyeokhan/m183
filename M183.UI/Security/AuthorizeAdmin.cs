using M183.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.UI.Security
{
    public class AuthorizeAdmin : AuthorizeAttribute
    {
        // Reference: https://stackoverflow.com/questions/11493873/how-to-implement-custom-authorize-attribute-for-the-following-case
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return BusinessUser.Current.IsInRole(Role.Admin);
        }
    }
}