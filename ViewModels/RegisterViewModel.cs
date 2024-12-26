using System.ComponentModel.DataAnnotations;

namespace ZamawianiePosiłkowOnline.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Wymagane imię")]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Wymagany adres email")]
        [EmailAddress]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wymagane hasło")]
        [StringLength(40, MinimumLength = 6,ErrorMessage = "The {0} must be at {2} and at max {1} characters")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Hasła do siebie nie pasują")]
        [Display(Name = "hasło")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Wymagane hasło")]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        public string ConfirmPassword { get; set; }
    }
}
