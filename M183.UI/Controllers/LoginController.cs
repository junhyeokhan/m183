using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using M183.BusinessLogic;
using M183.BusinessLogic.Models;
using M183.BusinessLogic.ViewModels;

namespace M183.UI.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel loginViewModel)
        {
            new Repository().TryLogIn(loginViewModel);

            if (BusinessUser.Current.Id > 0)
            {
                WebRequest request = (HttpWebRequest)WebRequest.Create("https://rest.nexmo.com/sms/json");
                string secret = "d6b2d10b06f2a697";
                string key = "5e4521b6";
                string code = new Random().Next(999999).ToString();
                string postData = string.Format("api_key={0}&api_secret={1}&to={2}&from=\"\"NEXMO\"\"&text=\"{3}\"", key, secret, BusinessUser.Current.MobileNumber, code);
                byte[] data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();     
                ViewBag.Message = responseString;
            }

            ModelState.AddModelError("Login", "Login was not successful.");

            return View(loginViewModel);
        }
    }
}