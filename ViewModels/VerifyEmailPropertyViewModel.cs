using System.ComponentModel.DataAnnotations;

namespace ZamawianiePosiłkowOnline.ViewModels
{
    public class VerifyEmailPropertyViewModel
    {
        [Required(ErrorMessage = "Wymagany adres email")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
