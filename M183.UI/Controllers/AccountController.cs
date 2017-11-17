using M183.BusinessLogic.Security;
using M183.BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Login(AccountViewModel accountViewModel)
        {
            if (accountViewModel.Username == "admin" && accountViewModel.Password == "test")
            {
                BusinessUser.Current.Username = accountViewModel.Username;
                BusinessUser.Current.Username = accountViewModel.Password;
                BusinessUser.Current.Roles.Add(new BusinessRole() { Name = "Admin" });
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else if (accountViewModel.Username == "user" && accountViewModel.Password == "test")
            {
                BusinessUser.Current.Username = accountViewModel.Username;
                BusinessUser.Current.Username = accountViewModel.Password;
                BusinessUser.Current.Roles.Add(new BusinessRole() { Name = "User" });
                return RedirectToAction("Index", "Dashboard", new { area = "User" });
            }
            else
            {
                ModelState.AddModelError("Login", "Wrogn credentials are entered.");
            }
            return View();
        }
    }
}