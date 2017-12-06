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
    public class SecurityManager : Manager
    {
        public void TryLogin(UserViewModel accountViewModel)
        {
            User user = db.User
                    .Where(u => u.Email == accountViewModel.Email &&
                        u.Password == accountViewModel.Password) //TODO: Hash password!
                    .FirstOrDefault();
            
            if (user != null)
            {
                LoginConfiguration loginConfiguration = user.Configurations
                        .OfType<LoginConfiguration>()
                        .FirstOrDefault();

                bool isAuthenticated = false;

                switch ((LoginMethod)loginConfiguration.LoginMethod)
                {
                    case LoginMethod.Default:
                        //No additional authentication
                        isAuthenticated = true;
                        break;
                }

                if (isAuthenticated)
                {
                    BusinessUser.Current.Id = user.Id;
                    BusinessUser.Current.Email = user.Email;
                    BusinessUser.Current.Roles = user.Roles
                            .Select(r => new BusinessRole() {
                                Id = r.Id,
                                Name = r.Name,
                            })
                            .ToList();
                }
            }
        }
    }
}
