using System.ComponentModel.DataAnnotations;

namespace E_Commerce.WebMVC.Models
{
    public class LoginModel
    {
        
        [Required(ErrorMessage ="Email required")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; } = null!;
    }
}
