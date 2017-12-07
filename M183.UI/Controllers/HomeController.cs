using M183.BusinessLogic;
using System.Web.Mvc;

namespace M183.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Introduction = new Repository().GetConfiguration("Home_Introduction");
            return View();
        }

        public ActionResult Feedback()
        {
            return View();
        }
    }
}