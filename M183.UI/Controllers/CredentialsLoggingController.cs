using M183.BusinessLogic;
using M183.BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.UI.Controllers
{
    public class CredentialsLoggingController : Controller
    {
        private static Repository repository = new Repository();

        public ActionResult Index()
        {
            List<LoggedAccountViewModel> accounts = /*repository.GetAllAccounts()*/null;
            return View(accounts);
        }

        public ActionResult Clear()
        {
            //repository.ClearAllAccounts();
            return RedirectToAction("Index");
        }
    }
}