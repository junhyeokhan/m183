using System.Web.Mvc;
using M183.BusinessLogic;
using M183.BusinessLogic.Models;
using System.Collections.Generic;
using M183.BusinessLogic.ViewModels;

namespace M183.UI.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            if (!BusinessUser.Current.IsAuthenticated || !BusinessUser.Current.HasRole(Role.Admin))
            {
                ModelState.AddModelError("Login", "You need to log in first to perform this action.");
                return RedirectToAction("Index", "Login", new { area = "" });
            }

            return View(new Repository().GetAllPosts("", false, true));
        }
        
        [HttpPost]
        public ActionResult SearchPost(string query, string submit)
        {
            // Here, SQL injection attack is prevented by using Linq. Reference: https://stackoverflow.com/a/473186
            List<PostViewModel> postViewModels = new List<PostViewModel>();
            Repository repository = new Repository();
            switch (submit)
            {
                case "Search":
                    postViewModels = repository.GetAllPosts(query, false, true);
                    break;
                case "Show all":
                    postViewModels = repository.GetAllPosts("", false, true);
                    break;
                default:
                    break;
            }
            ViewBag.SearchedQuery = query;
            return View("Index", postViewModels);
        }
    }
}