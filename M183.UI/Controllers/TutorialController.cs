using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.UI.Controllers
{
    public class TutorialController : Controller
    {
        // GET: Tutorial
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VigenereEncryption()
        {
            return View();
        }

        public ActionResult CesarEncryption()
        {
            return View();
        }
    }
}