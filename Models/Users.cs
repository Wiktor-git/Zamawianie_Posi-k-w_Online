using Microsoft.AspNetCore.Identity;

namespace ZamawianiePosiłkowOnline.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }

    }
}
