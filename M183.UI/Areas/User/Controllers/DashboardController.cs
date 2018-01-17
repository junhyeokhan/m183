using M183.BusinessLogic;
using M183.BusinessLogic.Models;
using M183.BusinessLogic.ViewModels;
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
            if (!BusinessUser.Current.IsAuthenticated || !BusinessUser.Current.HasRole(Role.User))
            {
                ModelState.AddModelError("Login", "You need to log in first to perform this action.");
                return RedirectToAction("Index", "Login");
            }

            return View(new Repository().GetPosts(BusinessUser.Current.Id));
        }
    }
}