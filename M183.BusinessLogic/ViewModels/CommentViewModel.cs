using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.BusinessLogic.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
