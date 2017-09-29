using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SingleSignOn.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public JsonResult SSOTokenSignIn()
        {
            bool isVerified = false;
            //Verify Token

            if (isVerified)
            {
                //
            }

            return Json("");
        }
    }
}