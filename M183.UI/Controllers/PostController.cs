using M183.BusinessLogic;
using M183.BusinessLogic.Models;
using M183.BusinessLogic.ViewModels;
using M183.UI.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace M183.UI.Controllers
{
    public class PostController : Controller
    {
        private static Repository repository = new Repository();

        [AuthorizeUser]
        [HttpGet]
        public ActionResult AddPost()
        {
            return View();
        }

        [AuthorizePostOwner]
        [HttpGet]
        public ActionResult EditPost(string postId)
        {
            int realId = repository.GetPostId(postId);
            return View(repository.GetPostDetail(realId));
        }

        [AuthorizePostOwner]
        [HttpPost]
        public ActionResult EditPost(PostViewModel postViewModel, string Id)
        {
            postViewModel.Id = repository.GetPostId(Id);
            repository.SavePost(postViewModel);
            return RedirectToAction("Index", "Dashboard", new { area = "user" });
        }

        [AuthorizeUser]
        [HttpPost]
        public ActionResult AddPost(PostViewModel postViewModel)
        {
            repository.SavePost(postViewModel);
            return RedirectToAction("Index", "Home");
        }

        [AuthorizePostOwner]
        [HttpGet]
        public ActionResult DeletePost(string postId)
        {
            int realId = repository.GetPostId(postId);
            if (repository.IsUserAuthorisedToPost(BusinessUser.Current.Id, realId))
            {
                repository.DeletePost(realId);
            }
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public ActionResult PostDetail(string postId)
        {
            int realId = repository.GetPostId(postId);
            return View(repository.GetPostDetail(realId));
        }

        [HttpPost]
        public ActionResult AddComment(PostViewModel postViewModel, string Id, string comment)
        {
            int realId = repository.GetPostId(Id);
            // Validate length (Controller level)
            if (comment.Length > 200 || comment.Length < 1)
            {
                ModelState.AddModelError("Comment", "Comment must be 1~200 characters long.");
                postViewModel.Comments = repository.GetComments(realId);
                return View("PostDetail", postViewModel);
            }

            // SQL Injection Prevention is done by LINQ and Entity Framework. Reference: https://stackoverflow.com/a/9079496
            repository.AddComment(realId, comment);
            return RedirectToAction("PostDetail", new { postId = MD5Hash.HashId(realId) });
        }
    }
}