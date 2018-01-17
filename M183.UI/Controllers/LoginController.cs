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
using System.Net.Mail;

namespace M183.UI.Controllers
{
    public class LoginController : Controller
    {
        private static Repository repository = new Repository();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            repository.TryLogIn(loginViewModel);
            if (BusinessUser.Current.IsBlocked)
            {
                ModelState.AddModelError("Login", "User account is blocked.");
            }
            else
            {
                if (BusinessUser.Current.Id > 0)
                {
                    try
                    {
                        string code = new Random().Next(999999).ToString();
                        switch (BusinessUser.Current.AuthenticationMethod)
                        {
                            case AuthenticationMethod.SMS:
                                {
                                    WebRequest request = (HttpWebRequest)WebRequest.Create("https://rest.nexmo.com/sms/json");
                                    string secret = "d6b2d10b06f2a697";
                                    string key = "5e4521b6";
                                    string postData = string.Format("api_key={0}&api_secret={1}&to={2}&from=M183&text=Your code: {3}\n", key, secret, BusinessUser.Current.MobileNumber, code);
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
                                    new Repository().AddToken(BusinessUser.Current.Id, code, DateTime.Now.AddMinutes(5));
                                    return View();
                                }
                            case AuthenticationMethod.Email:
                                {
                                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                                    {
                                        Credentials = new NetworkCredential("M183.Junhyeok.Han@gmail.com", "MySecretPassword"),
                                        EnableSsl = true,
                                    };
                                    smtpClient.Send("No-Reply@M183.com", BusinessUser.Current.EmailAddress, "M183 - Verification code for your accoun", "Your code: " + code);
                                    new Repository().AddToken(BusinessUser.Current.Id, code, DateTime.Now.AddMinutes(5));
                                    return View();
                                }
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        repository.SaveUserLog(BusinessUser.Current.Id, LogClass.Error, "Login", "There was an error while logging in user " + BusinessUser.Current.Username + ".\n" + ex.Message);
                        ModelState.AddModelError("Login", "There was an error while logging in." + ex.Message);
                    }
                    finally
                    {

                    }
                }

                ModelState.AddModelError("Login", "Login was not successful.");
            }

            return View("Index", loginViewModel);
        }

        [HttpPost]
        public ActionResult VerifyCode(LoginViewModel loginViewModel)
        {
            try
            {
                // Get IP address
                IPAddress ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList
                        .Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        .FirstOrDefault();

                if (ipAddress == null)
                {
                    throw new Exception("No network adapters with an IPv4 address in the system!");
                }

                repository.VerifyToken(BusinessUser.Current.Id, loginViewModel.Token, ipAddress.ToString());

                // Is user verified with the code?
                if (BusinessUser.Current.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                repository.SaveUserLog(BusinessUser.Current.Id, LogClass.Error, "Login", "There was an error while verifying the user " + BusinessUser.Current.Username + ".\n" + ex.Message);
                ModelState.AddModelError("Login", "There was an error while verifying the code." + ex.Message);
            }
            finally
            {
                // Otherwise add error message
                ModelState.AddModelError("Login", "Entered token does not correspond original code.");
            }

            return View("Login", loginViewModel);
        }

        public ActionResult Logout()
        {
            new Repository().TryLogOut();
            return RedirectToAction("Index", "Login");
        }
    }
}