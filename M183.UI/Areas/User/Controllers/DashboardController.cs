using M183.BusinessLogic;
using M183.BusinessLogic.Models;
using M183.BusinessLogic.ViewModels;
using M183.UI.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.UI.Areas.User.Controllers
{
    public class DashboardController : Controller
    {
        [AuthorizeUser]
        // GET: User/Dashboard
        public ActionResult Index()
        {
            return View(new Repository().GetPosts(BusinessUser.Current.Id));
        }
    }
}