using M183.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.BusinessLogic.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatedPassword { get; set; }

        public LoginMethod LoginMethod { get; set; }
    }
}
