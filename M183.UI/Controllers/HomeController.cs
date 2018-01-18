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

        /*
         * Answers for the questions
         * 1) Hash function Reference: https://security.stackexchange.com/questions/211/how-to-securely-hash-passwords/
         * I chose PBKDF2 because 
         * Has been specified for a long time, seems unscathed for now.
         * Is already implemented in various framework (e.g. it is provided with .NET).
         * Highly configurable (although some implementations do not let you choose the hash function, e.g. the one in .NET is for SHA-1 only).
         * Received NIST blessings (modulo the difference between hashing and key derivation; see later on).
         * Configurable output length (again, see later on).
         */
    }
}