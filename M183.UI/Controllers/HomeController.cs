using System.Web.Mvc;
using M183.BusinessLogic;
using M183.BusinessLogic.Models;

namespace M183.UI.Controllers
{
    public class HomeController : Controller
    {
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

            // Otherwise get all published posts and show them to user
            return View(new Repository().GetAllPosts("", false, false));
        }
    }
}