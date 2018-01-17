using M183.BusinessLogic.Models;

namespace M183.BusinessLogic.ViewModels
{
    public class RegisterViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public AuthenticationMethod AuthenticationMethod { get; set; }
    }
}
