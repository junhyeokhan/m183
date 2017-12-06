using M183.BusinessLogic.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.UI.Areas.User.Controllers
{
    public class DashboardController : Controller
    {
        // GET: User/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}