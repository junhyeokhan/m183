using M183.BusinessLogic;
using M183.BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.UI.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(RegisterViewModel registerViewModel)
        {
            string message = new Repository().TryRegister(registerViewModel);

            if (string.IsNullOrEmpty(message))
            {
                return RedirectToAction("Index", "Login");
            }

            ModelState.AddModelError("Register", message);

            return View(registerViewModel);
        }
    }
}