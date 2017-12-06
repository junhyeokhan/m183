using System.Web.Mvc;
using M183.BusinessLogic.Managers;
using M183.BusinessLogic.Security;
using M183.BusinessLogic.ViewModels;

namespace M183.UI.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserViewModel userViewModel)
        {
            //Try Login
            new SecurityManager().TryLogin(userViewModel);

            //Login succeeded
            if (BusinessUser.Current.Id > 0)
            {
                if (BusinessUser.Current.Roles.Contains(BusinessRole.Admin))
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "user" });
                }
            }

            //Login failed
            ModelState.AddModelError("Login", "Login has failed. Please retry.");

            return View(userViewModel);
        }
    }
}