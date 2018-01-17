using System;
using System.Linq;
using M183.DataAccess;
using M183.BusinessLogic.Models;
using M183.BusinessLogic.ViewModels;
using System.Collections.Generic;

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
                UserStatus userStatus = user.UserStatues == null ?
                        null :
                        user.UserStatues.Where(us => us.DeletedOn == null).OrderByDescending(us => us.CreatedOn).FirstOrDefault();

                // Is last status blocked?
                if (userStatus != null)
                {
                    BusinessUser.Current.IsBlocked = userStatus.Status == (int)Status.Blocked;
                }
                else
                {
                    userStatus = new UserStatus()
                    {
                        CreatedOn = DateTime.Now,
                        DeletedOn = null,
                        ModifiedOn = null,
                        Status = (int)Status.Default,
                        User = user,
                    };

                    db.UserStatus.Add(userStatus);
                    db.SaveChanges();
                }

                if (!BusinessUser.Current.IsBlocked)
                {
                    if (loginViewModel.Password == user.Password)
                    {
                        BusinessUser.Current.Id = user.Id;
                        BusinessUser.Current.Username = user.UserName;
                        BusinessUser.Current.MobileNumber = user.MobileNumber;
                        BusinessUser.Current.AuthenticationMethod = (AuthenticationMethod)user.AuthenticationMode;

                        isAuthenticated = true;
                    }
                    // Is password wrong?
                    else
                    {
                        // Add log
                        SaveUserLog(user.Id, LogClass.FailedLoginAttempt, "Login", "False password is entered.");
                        AddFailedAttempt(user.Id);
                    }
                }
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
                        SaveUserLog(userId, LogClass.SuccessfullLogin, "Login", "User" + user.UserName + " is logged in.");

                        //TODO: Add session Id
                        // Add user login
                        UserLogin userLogin = new UserLogin() { User = user, SessionId = "", CreatedOn = DateTime.Now, DeletedOn = null, ModifiedOn = null, IP = ipAddress };
                        db.UserLogin.Add(userLogin);

                        db.SaveChanges();
                    }
                    // Is entered token false?
                    else
                    {
                        AddFailedAttempt(userId);
                    }
                }
            }
        }

        public void SaveUserLog(int userId, LogClass logClass, string action, string message)
        {
            User user = db.User.Where(u => u.Id == userId).FirstOrDefault();

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

        public void AddFailedAttempt(int userId)
        {
            SaveUserLog(userId, LogClass.FailedLoginAttempt, "Login", "False code is entered.");

            User user = db.User.Where(u => u.Id == userId).FirstOrDefault();

            // Did last all three attempts all fail?
            if (user.UserLogs.OrderByDescending(ul => ul.Timestamp).Take(3).All(ul => ul.Class == (int)LogClass.FailedLoginAttempt))
            {
                UserStatus userStatus = new UserStatus()
                {
                    CreatedOn = DateTime.Now,
                    DeletedOn = null,
                    ModifiedOn = null,
                    Status = (int)Status.Blocked,
                    User = user,
                };

                db.UserStatus.Add(userStatus);
                db.SaveChanges();
            }
        }

        public User GetUser(int userId)
        {
            return db.User.Where(u => u.Id == userId).FirstOrDefault();
        }

        #region Posts
        public List<PostViewModel> GetAllPosts(string query)
        {
            return db.Posts
                    .OrderByDescending(p => p.CreatedOn)
                    .Select(p => new PostViewModel()
                    {
                        Id = p.Id,
                        Content = p.Content,
                        CreatedOn = p.CreatedOn,
                        DeletedOn = p.DeletedOn,
                        Description = p.Description,
                        EditedOn = p.EditedOn,
                        Title = p.Title,
                    })
                    .Where(p => string.IsNullOrEmpty(query) ?
                        true :
                        p.Title.ToLower().Contains(query.ToLower()) ||
                            p.Description.ToLower().Contains(query.ToLower()) ||
                            p.Content.ToLower().Contains(query.ToLower()))
                    .ToList();
        }

        public List<PostViewModel> GetPosts(int userId)
        {
            return db.Posts
                    .Where(p => p.User.Id == userId && p.DeletedOn == null)
                    .Select(p => new PostViewModel()
                    {
                        Id = p.Id,
                        Content = p.Content,
                        CreatedOn = p.CreatedOn,
                        DeletedOn = p.DeletedOn,
                        Description = p.Description,
                        EditedOn = p.EditedOn,
                        Title = p.Title,
                    })
                    .ToList();
        }
        public void SavePost(PostViewModel postViewModel)
        {
            // Check if user is logged in
            User user = GetUser(BusinessUser.Current.Id);

            if (user != null)
            {
                // Check if the post exists already
                Post post = db.Posts.Where(p => p.Id == postViewModel.Id).FirstOrDefault();

                // Is the post new one?
                if (post == null)
                {
                    // Create new post
                    post = new Post()
                    {
                        User = user,
                        Title = postViewModel.Title,
                        CreatedOn = DateTime.Now,
                        DeletedOn = null,
                        EditedOn = null,
                        Content = postViewModel.Content,
                        Description = postViewModel.Description,
                    };

                    db.Posts.Add(post);
                    db.SaveChanges();
                }
                // Is the post existing one?
                else
                {

                }
            }
        }
        public void DeletePost(int postId)
        {
            Post post = db.Posts.Where(p => p.Id == postId).FirstOrDefault();

            if (post != null)
            {
                post.DeletedOn = DateTime.Now;
                db.SaveChanges();
            }
        }
        #endregion
    }
}
