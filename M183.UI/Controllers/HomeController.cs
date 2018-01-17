using System.Web.Mvc;
using M183.BusinessLogic.Models;
using M183.BusinessLogic.ViewModels;
using System.Collections.Generic;
using M183.BusinessLogic;

namespace M183.UI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            // Is user already authenticated?
            if (BusinessUser.Current.IsAuthenticated)
            {
                // Redirect to dashboard according to the role
                if (BusinessUser.Current.Roles.Contains(Role.Admin))
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "admin" });
                }
                else if (BusinessUser.Current.Roles.Contains(Role.User))
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "user" });
                }
            }
            
            return View(new Repository().GetAllPosts("", true));
        }
    }
}