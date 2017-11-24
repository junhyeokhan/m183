using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.BusinessLogic.ViewModels
{
    public class FeedbackViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Feedback { get; set; }
    }
}
