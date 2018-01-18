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
        [AuthorizeUser]
        [HttpGet]
        public ActionResult AddPost()
        {
            return View();
        }

        [AuthorizeUser]
        [HttpPost]
        public ActionResult AddPost(PostViewModel postViewModel)
        {
            new Repository().SavePost(postViewModel);
            return RedirectToAction("Index", "Home");
        }

        [AuthorizePostOwner]
        [HttpGet]
        public ActionResult DeletePost(int postId)
        {
            Repository repository = new Repository();
            if (repository.IsUserAuthorisedToPost(BusinessUser.Current.Id, postId))
            {
                repository.DeletePost(postId);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult PostDetail(int postId)
        {
            return View(new Repository().GetPostDetail(postId));
        }

        [HttpPost]
        public ActionResult AddComment(PostViewModel postViewModel, int Id, string comment)
        {
            Repository repository = new Repository();
            // Validate length (Controller level)
            if (comment.Length > 200 || comment.Length < 1)
            {
                ModelState.AddModelError("Comment", "Comment must be 1~200 characters long.");
                postViewModel.Comments = repository.GetComments(Id);
                return View("PostDetail", postViewModel);
            }

            // SQL Injection Prevention is done by LINQ and Entity Framework. Reference: https://stackoverflow.com/a/9079496
            repository.AddComment(Id, comment);
            return RedirectToAction("PostDetail", new { postId = Id });
        }
    }
}