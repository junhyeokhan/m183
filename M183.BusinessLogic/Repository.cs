using M183.BusinessLogic.Models;
using M183.BusinessLogic.ViewModels;
using M183.DataAccess;
using System;
using System.Collections.Generic;
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
                };
                db.User.Add(user);
                db.SaveChanges();
            }

            return message;
        }

        public void TryLogIn(LoginViewModel loginViewModel)
        {
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
                }
            }
        }
    }
}
