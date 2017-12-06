using M183.BusinessLogic.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;

namespace M183.UI.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            if (!BusinessUser.Current.Roles.Contains(BusinessRole.Admin))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }
    }
}