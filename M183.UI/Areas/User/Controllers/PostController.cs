using M183.BusinessLogic;
using M183.BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.UI.Areas.User.Controllers
{
    public class PostController : Controller
    {
        // GET: User/Post
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPost(PostViewModel postViewModel)
        {
            new Repository().SavePost(postViewModel);
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public ActionResult DeletePost(int postId)
        {
            //TODO: Authorize user
            new Repository().DeletePost(postId);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}