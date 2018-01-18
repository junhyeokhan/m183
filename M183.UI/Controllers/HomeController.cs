using System.Web.Mvc;
using M183.BusinessLogic;
using M183.BusinessLogic.Models;

namespace M183.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Get all published posts and show them to user
            return View(new Repository().GetAllPosts("", false, false));
        }

        public ActionResult ApiTest()
        {
            return View();
        }
    }
}