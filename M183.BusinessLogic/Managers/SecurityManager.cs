using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M183.BusinessLogic.ViewModels;
using M183.DataAccess.Models;
using M183.DataAccess.Models.Configurations;
using M183.BusinessLogic.Models;
using M183.BusinessLogic.Security;

namespace M183.BusinessLogic.Managers
{
    public class UserManager : Manager
    {
        public void TryLogin(UserViewModel accountViewModel)
        {
            User user = db.User
                    .Where(u => u.Email == accountViewModel.Email &&
                        u.Password == accountViewModel.Password) //TODO: Hash password!
                    .FirstOrDefault();
            
            if (user != null)
            {
                LoginConfiguration loginConfiguration = user.Configurations == null ?
                        null :
                        user.Configurations
                            .OfType<LoginConfiguration>()
                            .FirstOrDefault();

                bool isAuthenticated = false;

                if (loginConfiguration != null)
                {
                    switch ((LoginMethod)loginConfiguration.LoginMethod)
                    {
                        case LoginMethod.Default:
                            //No additional authentication
                            isAuthenticated = true;
                            break;
                    }
                }
                else
                {
                    isAuthenticated = true;
                }


                if (isAuthenticated)
                {
                    BusinessUser.Current.Id = user.Id;
                    BusinessUser.Current.Email = user.Email;
                    BusinessUser.Current.Roles = user.Roles
                            .ToList()
                            .Select(r => (BusinessRole)r.RoleType)
                            .ToList();
                }
            }
        }

        public bool TryRegister(UserViewModel userViewModel)
        {
            bool result = false;
            
            //Check if any user has same email address
            User user = db.User.Where(u => u.Email == userViewModel.Email).FirstOrDefault();

            //No user with same email address is found.
            if (user == null)
            {
                try
                {
                    //Create new user with entered credentials.
                    user = new User()
                    {
                        Email = userViewModel.Email,
                        Password = userViewModel.Password,
                        Roles = db.Role.Where(r => r.RoleType == (int)BusinessRole.Default).ToList(),
                    };
                    //Save new user into the database.
                    db.User.Add(user);
                    db.SaveChanges();
                    result = true;
                }
                catch
                {
                    result = false;
                }
                finally
                {

                }
            }
            return result;
        }
    }
}
