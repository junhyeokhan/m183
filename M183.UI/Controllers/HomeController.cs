using System.Web.Mvc;
using M183.BusinessLogic;

namespace M183.UI.Controllers
{
    public class HomeController : Controller
    {
        // Render homepage with posts
        public ActionResult Index()
        {
            // Get all published posts and show them to user
            return View(new Repository().GetAllPosts("", false, false));
        }

        /* Answers for the questions
         *  1) Why did you decide to use the Hash Algorithm (Username & Password)?
         *      I have decided to use SHA-3 first, since it was the newest one. 
         *      However, since SHA-3 is not supported yet, I have chosen another one - PBKDF2
         *      The reasons why I chose it were:
         *          It has been specified for a long time and seems unscathed for now.
         *          It is already implemented in various framework. (e.g. .NET)
         *          It is highly configurable with different variables.
         *          It received NIST blessings.
         *          It's output length can be configured.
         *  2,3) In User table, there is reserved column for IP-Address. Which attack does it prevent?
         *  Explain how these attacks work and how the countermeasures prevent the attacks.
         *      Brute force attack: It can block the IP-Address after certain tries from the same IP-Address.
         *      Phishing: We can notify/ask for verification when different IP-Address than the user normally uses has been used to log in.
         */
    }
}