using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZamawianiePosiłkowOnline.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Wymagany poprawny adres email")]
        [EmailAddress]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Wymagane poprawne hasło")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}
