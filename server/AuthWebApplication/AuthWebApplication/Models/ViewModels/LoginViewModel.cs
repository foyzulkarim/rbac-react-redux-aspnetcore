using System.ComponentModel.DataAnnotations;

namespace AuthWebApplication.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LogoutViewModel
    {
        [Required]
        public string Jti { get; set; }
    }
}