using M183.BusinessLogic;
using M183.BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.UI.Controllers
{
    public class KeyLoggingController : Controller
    {
        private static Repository repository = new Repository();

        public ActionResult Index()
        {
            List<LoggedTextViewModel> logs = repository.GetAllKeyLogs();
            return View(logs);
        }

        public ActionResult Clear()
        {
            repository.ClearAllKeyLogs();
            return RedirectToAction("Index");
        }
    }
}