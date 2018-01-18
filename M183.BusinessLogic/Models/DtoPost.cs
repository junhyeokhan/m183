using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M183.UI.BusinessLogic.Models
{
    public class DtoPost
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string[] Comments { get; set; }
    }
}