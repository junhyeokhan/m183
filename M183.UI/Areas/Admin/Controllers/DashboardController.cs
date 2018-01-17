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

            return View(new Repository().GetAllPosts("", false));
        }
        
        [HttpPost]
        public ActionResult SearchPost(string query, string submit)
        {
            //TODO: SQL Injection Prevention?
            List<PostViewModel> postViewModels = new List<PostViewModel>();
            Repository repository = new Repository();
            switch (submit)
            {
                case "Search":
                    postViewModels = repository.GetAllPosts(query, false);
                    break;
                case "Show all":
                    postViewModels = repository.GetAllPosts("", false);
                    break;
                default:
                    break;
            }
            return View("Index", postViewModels);
        }
    }
}