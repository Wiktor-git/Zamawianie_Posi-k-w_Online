using System.ComponentModel.DataAnnotations;

namespace ZamawianiePosiłkowOnline.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Wymagany adres email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wymagane hasło")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "The {0} must be at {2} and at max {1} characters")]
        [DataType(DataType.Password)]
        [Compare("ConfirmNewPassword", ErrorMessage = "Hasła do siebie nie pasują")]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Wymagane hasło")]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź nowe hasło")]
        public string ConfirmNewPassword { get; set; }
    }
}
