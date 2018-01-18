using System.Web.Mvc;
using M183.BusinessLogic;
using M183.BusinessLogic.ViewModels;

namespace M183.UI.Controllers
{
    public class RegisterController : Controller
    {
        // Render registration form
        public ActionResult Index()
        {
            return View();
        }

        // Regiser user with submitted data
        [HttpPost]
        public ActionResult Index(RegisterViewModel registerViewModel)
        {
            // Get error message while trying to register the user
            string message = new Repository().TryRegister(registerViewModel);

            // No error message was returned
            if (string.IsNullOrEmpty(message))
            {
                // Redirect to login form
                return RedirectToAction("Index", "Login", new { area = "" });
            }

            // Show error message to the user
            ModelState.AddModelError("Register", message);
            return View(registerViewModel);
        }
    }
}