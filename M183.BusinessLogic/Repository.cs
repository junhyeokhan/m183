using System;
using System.Linq;
using M183.DataAccess;
using M183.BusinessLogic.Models;
using M183.BusinessLogic.ViewModels;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Runtime.Remoting.Contexts;
using M183.UI.BusinessLogic.Models;

namespace M183.BusinessLogic
{
    public class Repository
    {
        /// <summary>
        /// This method tries to register a new user
        /// </summary>
        /// <param name="registerViewModel">Necessary data of user as RegisterViewModel</param>
        /// <returns>String.Empty if there was no error, error message if there was an error, as string</returns>
        public string TryRegister(RegisterViewModel registerViewModel)
        {
            string message = string.Empty;

            using (DatabaseContext db = new DatabaseContext())
            {
                // Check if there is any user with same username
                User user = db.User
                           .Where(u => u.UserName == registerViewModel.Username)
                           .FirstOrDefault();
                
                // User with same username already exists
                if (user != null)
                {
                    message = "A user with entered username already exists. Please entere another username.";
                }
                // No user with same username exists
                else
                {
                    // Create new user with submitted data
                    user = new User()
                    {
                        UserName = registerViewModel.Username,
                        Password = registerViewModel.Password,
                        MobileNumber = registerViewModel.MobileNumber,
                        AuthenticationMode = (int)registerViewModel.AuthenticationMethod,
                        EmailAddress = registerViewModel.EmailAddress,
                    };
                    db.User.Add(user);

                    // Add user role as default
                    UserRole userRole = new UserRole()
                    {
                        Role = (int)Role.User,
                        User = user
                    };
                    db.UserRole.Add(userRole);

                    // Save all changes
                    db.SaveChanges();
                }
            }
            return message;
        }

        /// <summary>
        /// This method checks if a user is authorised to a post
        /// </summary>
        /// <param name="userId">Identifier of the user</param>
        /// <param name="postId">Identifier of the post</param>
        /// <returns>True if user was authorised, false if user was not authorised, as bool</returns>
        public bool IsUserAuthorisedToPost(int userId, int postId)
        {
            bool isAuthorised = false;
            using (DatabaseContext db = new DatabaseContext())
            {
                User user = db.User.Where(u => u.Id == userId).FirstOrDefault();
                Post post = db.Post.Where(p => p.Id == postId).FirstOrDefault();
                if (user != null && post != null)
                {
                    isAuthorised = user.Posts.Contains(post);
                }
            }
            return isAuthorised;
        }

        /// <summary>
        /// This method tries to log in with submitted credentials
        /// </summary>
        /// <param name="loginViewModel">Credentials as LoginViewModel</param>
        /// <returns>True if a user with submitted username and password was found, false if user was not found, as bool</returns>
        public bool TryLogIn(LoginViewModel loginViewModel)
        {
            bool isAuthenticated = false;
            using (DatabaseContext db = new DatabaseContext())
            {
                User user = db.User.Where(u => u.UserName == loginViewModel.Username).FirstOrDefault();
                if (user != null)
                {
                    UserStatus userStatus = user.UserStatues?.Where(us => us.DeletedOn == null).OrderByDescending(us => us.CreatedOn).FirstOrDefault();

                    // Is last status blocked?
                    if (userStatus != null)
                    {
                        BusinessUser.Current.IsBlocked = userStatus.Status == (int)UserStatusCode.Blocked;
                    }
                    else
                    {
                        userStatus = new UserStatus()
                        {
                            CreatedOn = DateTime.Now,
                            DeletedOn = null,
                            ModifiedOn = null,
                            Status = (int)UserStatusCode.Default,
                            User = user,
                        };

                        db.UserStatus.Add(userStatus);
                        db.SaveChanges();
                    }

                }

                if (!BusinessUser.Current.IsBlocked)
                {
                    if (loginViewModel.Password == user.Password)
                    {
                        BusinessUser.Current.Id = user.Id;
                        BusinessUser.Current.Username = user.UserName;
                        BusinessUser.Current.MobileNumber = user.MobileNumber;
                        BusinessUser.Current.AuthenticationMethod = (AuthenticationMethod)user.AuthenticationMode;
                        BusinessUser.Current.EmailAddress = user.EmailAddress;

                        // Add user login record
                        UserLogin userLogin = new UserLogin()
                        {

                        };

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

        public void TryLogOut()
        {


            // Reset current session
            BusinessUser.Current.Logout();
        }

        public void AddToken(int userId, string code, DateTime expiry)
        {
            using (DatabaseContext db = new DatabaseContext())
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
        }


        public void VerifyToken(int userId, string code, string ipAddress)
        {
            using (DatabaseContext db = new DatabaseContext())
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

                            //TODO: Update Session Id
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
        }

        public void SaveUserLog(int userId, LogClass logClass, string action, string message)
        {
            using (DatabaseContext db = new DatabaseContext())
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
        }

        public void AddFailedAttempt(int userId)
        {
            SaveUserLog(userId, LogClass.FailedLoginAttempt, "Login", "False code is entered.");

            using (DatabaseContext db = new DatabaseContext())
            {
                User user = db.User.Where(u => u.Id == userId).FirstOrDefault();
                // Did last all three attempts all fail?
                if (user.UserLogs.OrderByDescending(ul => ul.Timestamp).Take(3).All(ul => ul.Class == (int)LogClass.FailedLoginAttempt))
                {
                    // Block the user
                    UserStatus userStatus = new UserStatus()
                    {
                        CreatedOn = DateTime.Now,
                        DeletedOn = null,
                        ModifiedOn = null,
                        Status = (int)UserStatusCode.Blocked,
                        User = user,
                    };
                    db.UserStatus.Add(userStatus);
                    db.SaveChanges();
                }
            }
        }

        public List<PostViewModel> GetAllPosts(string query, bool onlyPublished, bool alsoDeleted)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.Post
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
                        IsPublished = p.PostStatuses.OrderByDescending(ps => ps.Timestamp).FirstOrDefault().Status == (int)PostStatusCode.Published
                    })
                    .Where(p => string.IsNullOrEmpty(query) ?
                        true :
                        p.Title.ToLower().Contains(query.ToLower()) ||
                            p.Description.ToLower().Contains(query.ToLower()) ||
                            p.Content.ToLower().Contains(query.ToLower()))
                    .Where(p => onlyPublished ?
                        p.IsPublished :
                        true)
                    .Where(p => alsoDeleted ?
                        true :
                        p.DeletedOn == null)
                    .ToList();
            }
        }

        public List<PostViewModel> GetPosts(int userId)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                return db.Post
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
        }
        public PostViewModel GetPostDetail(int postId)
        {
            PostViewModel postViewModel = new PostViewModel();
            using (DatabaseContext db = new DatabaseContext())
            {
                Post post = db.Post.Where(p => p.Id == postId && p.DeletedOn == null).FirstOrDefault();
                if (post != null)
                {
                    postViewModel.Id = post.Id;
                    postViewModel.Title = post.Title;
                    postViewModel.Description = post.Description;
                    postViewModel.CreatedOn = post.CreatedOn;
                    postViewModel.EditedOn = post.EditedOn;
                    postViewModel.Content = post.Content;
                    postViewModel.Comments = GetComments(postViewModel.Id);
                    postViewModel.IsPublished = post.PostStatuses.OrderByDescending(ps => ps.Timestamp).FirstOrDefault().Status == (int)PostStatusCode.Published;
                }
            }
            return postViewModel;
        }
        public List<CommentViewModel> GetComments(int postId)
        {
            List<CommentViewModel> comments = new List<CommentViewModel>();
            using (DatabaseContext db = new DatabaseContext())
            {
                Post post = db.Post.Where(p => p.Id == postId).FirstOrDefault();

                if (post != null)
                {
                    comments = post.Comments
                            .Select(c => new CommentViewModel()
                            {
                                Id = c.Id,
                                CreatedOn = c.CreatedOn,
                                Text = c.Text,
                            })
                            .ToList();
                }
            }
            return comments;
        }
        public void SavePost(PostViewModel postViewModel)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                // Check if user is logged in
                User user = db.User.Where(u => u.Id == BusinessUser.Current.Id).FirstOrDefault();

                if (user != null)
                {
                    // Check if the post exists already
                    Post post = db.Post.Where(p => p.Id == postViewModel.Id).FirstOrDefault();

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
                        user.Posts.Add(post);

                        // Add initial post status
                        PostStatus postStatus = new PostStatus()
                        {
                            Post = post,
                            Timestamp = DateTime.Now,
                            Status = postViewModel.IsPublished ?
                                (int)PostStatusCode.Published :
                                (int)PostStatusCode.Saved,
                        };
                        db.PostStatus.Add(postStatus);

                        db.SaveChanges();
                    }
                    // Is the post existing one?
                    else
                    {
                        post.Title = postViewModel.Title;
                        post.EditedOn = DateTime.Now;
                        post.Content = postViewModel.Content;
                        post.Description = postViewModel.Description;
                        
                        if (post.PostStatuses.OrderByDescending(ps => ps.Timestamp).FirstOrDefault().Status == (int)PostStatusCode.Published &&
                                !postViewModel.IsPublished)
                        {
                            PostStatus postStatus = new PostStatus()
                            {
                                Post = post,
                                Status = (int)PostStatusCode.Saved,
                                Timestamp = DateTime.Now,
                            };
                            post.PostStatuses.Add(postStatus);
                        }
                        else if (post.PostStatuses.OrderByDescending(ps => ps.Timestamp).FirstOrDefault().Status != (int)PostStatusCode.Published &&
                                postViewModel.IsPublished)
                        {
                            PostStatus postStatus = new PostStatus()
                            {
                                Post = post,
                                Status = (int)PostStatusCode.Published,
                                Timestamp = DateTime.Now,
                            };
                            post.PostStatuses.Add(postStatus);
                        }

                        db.SaveChanges();
                    }
                }
            }
        }

        public void DeletePost(int postId)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                Post post = db.Post.Where(p => p.Id == postId).FirstOrDefault();

                if (post != null)
                {
                    post.DeletedOn = DateTime.Now;
                    db.SaveChanges();
                }
            }
        }

        public void AddComment(int postId, string text)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                Post post = db.Post.Where(p => p.Id == postId).FirstOrDefault();

                if (post != null)
                {
                    Comment comment = new Comment()
                    {
                        CreatedOn = DateTime.Now,
                        Text = text,
                        Post = post,
                    };
                    db.Comment.Add(comment);
                    db.SaveChanges();
                }
            }
        }

        public IEnumerable<DtoPost> GetAllDtoPosts()
        {
            IEnumerable<DtoPost> dtoPosts = new List<DtoPost>();
            using (DatabaseContext db = new DatabaseContext())
            {
                List<Post> posts = db.Post
                    .Where(p => p.DeletedOn == null && 
                        p.PostStatuses
                            .OrderByDescending(ps => ps.Timestamp)
                            .FirstOrDefault().Status == (int)PostStatusCode.Published)
                    .ToList();
                dtoPosts = posts
                    .Select(p => new DtoPost()
                    {
                        Title = p.Title,
                        Description = p.Description,
                        Content = p.Content,
                        Comments = p.Comments.Select(c => c.Text).ToArray()
                    })
                    .ToList();
            }
            return dtoPosts;
        }

        public DtoPost GetDtoPost(int postId)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                Post post = db.Post
                    .Where(p => p.Id == postId &&
                        p.DeletedOn == null &&
                        p.PostStatuses
                            .OrderByDescending(ps => ps.Timestamp)
                            .FirstOrDefault().Status == (int)PostStatusCode.Published)
                    .FirstOrDefault();

                return post == null ? null : new DtoPost() { Title = post.Title, Comments = post.Comments.Select(c => c.Text).ToArray(), Content = post.Content, Description = post.Description };
            }
        }
    }
}
