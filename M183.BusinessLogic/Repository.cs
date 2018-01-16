using M183.BusinessLogic.Models;
using M183.BusinessLogic.ViewModels;
using M183.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.BusinessLogic
{
    public class Repository
    {
        private static DatabaseContext db = new DatabaseContext();

        public string TryRegister(RegisterViewModel registerViewModel)
        {
            string message = string.Empty;

            User user = db.User
                    .Where(u => u.UserName == registerViewModel.Username)
                    .FirstOrDefault();

            if (user != null)
            {
                message = "A user with entered username already exists. Please entere another username.";
            }
            else
            {
                user = new User()
                {
                    UserName = registerViewModel.Username,
                    Password = registerViewModel.Password,
                    MobileNumber = registerViewModel.MobileNumber,
                    AuthenticationMode = (int)registerViewModel.AuthenticationMethod,
                };
                db.User.Add(user);
                db.SaveChanges();
            }

            return message;
        }

        public bool TryLogIn(LoginViewModel loginViewModel)
        {
            bool isAuthenticated = false;

            User user = db.User
                    .Where(u => u.UserName == loginViewModel.Username)
                    .FirstOrDefault();

            if (user != null)
            {
                if (loginViewModel.Password == user.Password)
                {
                    BusinessUser.Current.Id = user.Id;
                    BusinessUser.Current.Username = user.UserName;
                    BusinessUser.Current.MobileNumber = user.MobileNumber;
                    BusinessUser.Current.AuthenticationMethod = (AuthenticationMethod)user.AuthenticationMode;
                }
                isAuthenticated = true;
            }

            return isAuthenticated;
        }

        public void AddToken(int userId, string code, DateTime expiry)
        {
            User user = db.User
                    .Where(u => u.Id == userId)
                    .FirstOrDefault();

            if (user != null)
            {
                Token token = new Token()
                {
                    User = user,
                    Expiry = expiry,
                    TokenString = code,
                };
                db.Token.Add(token);
                db.SaveChanges();
            }

        }

        public void VerifyToken(int id, string code)
        {
            // Get the last non-expired token
            Token token = db.Token
                    .Where(t => t.Expiry >= DateTime.Now)
                    .OrderByDescending(t => t.Expiry)
                    .FirstOrDefault();

            // Is there any token as queried?
            if (token != null)
            {
                // Does the token have same code as submitted code?
                if (token.TokenString == code)
                {
                    // Mark current user (session) as verified.
                    BusinessUser.Current.IsVerified = true;
                }
            }
        }
    }
}
