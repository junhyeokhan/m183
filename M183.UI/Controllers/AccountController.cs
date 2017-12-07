using System.Web.Mvc;
using M183.BusinessLogic.Managers;
using M183.BusinessLogic.Security;
using M183.BusinessLogic.ViewModels;

namespace M183.UI.Controllers
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

        [HttpPost]
        public ActionResult Login(UserViewModel userViewModel)
        {
            //Try Login
            new UserManager().TryLogin(userViewModel);

            //Login succeeded
            if (BusinessUser.Current.Id > 0)
            {
                if (BusinessUser.Current.Roles.Contains(BusinessRole.Admin))
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "user" });
                }
            }

            //Login failed
            ModelState.AddModelError("Login", "Login has failed. Please retry.");

            return View(userViewModel);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserViewModel userViewModel)
        {
            //TODO: Add validation (e.g. Password rule)

            //Try registering.
            if (new UserManager().TryRegister(userViewModel))
            {
                //Registration succeeded.
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            //Registration failed.
            return View();
        }

        public JsonResult SSOTokenSignIn()
        {
            var id_token = Request["idtoken"];
            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=" + id_token);
            var postData = "id_token=" + id_token;
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return Json(responseString);
        }
    }
}