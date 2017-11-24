using M183.BusinessLogic;
using M183.BusinessLogic.Security;
using M183.BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.UI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if (BusinessUser.Current.Id > 0)
            {

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public ActionResult Feedback()
        {
            return View();
        }
    }
}