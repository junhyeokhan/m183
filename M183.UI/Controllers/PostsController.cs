using M183.BusinessLogic;
using M183.UI.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace M183.UI.Controllers
{
    public class PostsController : ApiController
    {
        public IEnumerable<DtoPost> GetAllPosts()
        {
            return new Repository().GetAllDtoPosts();
        }

        public DtoPost GetPost(int id)
        {
            return new Repository().GetDtoPost(id);
        }
    }
}
