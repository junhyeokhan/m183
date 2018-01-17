using System;
using System.Linq;
using M183.DataAccess;
using M183.BusinessLogic.Models;
using M183.BusinessLogic.ViewModels;

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

                UserRole userRole = new UserRole()
                {
                    Role = (int)Role.User,
                    User = user
                };
                db.UserRole.Add(userRole);

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

        public void VerifyToken(int userId, string code, string ipAddress)
        {
            User user = db.User.Where(u => u.Id == userId).FirstOrDefault();

            if (user != null)
            {
                // Get the last non-expired token
                Token token = user.Tokens
                        .Where(t => t.Expiry >= DateTime.Now && !t.Deleted)
                        .OrderByDescending(t => t.Expiry)
                        .FirstOrDefault();

                // Is there any token as queried?
                if (token != null)
                {
                    // Does the token have same code as submitted code?
                    if (token.TokenString == code)
                    {
                        // Mark current user (session) as verified
                        BusinessUser.Current.IsVerified = true;

                        // Add user's roles
                        BusinessUser.Current.Roles = user.UserRoles
                                .Select(r => (Role)r.Role)
                                .ToList();

                        // Delete used token
                        token.Deleted = true;

                        // Add user log
                        SaveUserLog(LogClass.Information, "Login", "User" + user.UserName + " is logged in.");

                        //TODO: Add session Id
                        // Add user login
                        UserLogin userLogin = new UserLogin() { User = user, SessionId = "", CreatedOn = DateTime.Now, DeletedOn = null, ModifiedOn = null, IP = ipAddress };
                        db.UserLogin.Add(userLogin);

                        db.SaveChanges();
                    }
                }
            }
        }

        public void SaveUserLog(LogClass logClass, string action, string message)
        {
            User user = db.User.Where(u => u.Id == BusinessUser.Current.Id).FirstOrDefault();

            UserLog userLog = new UserLog()
            {
                Action = action,
                Class = (int)logClass,
                Message = message,
                Timestamp = DateTime.Now,
                User = user
            };

            db.UserLog.Add(userLog);
            db.SaveChanges();
        }
    }
}
