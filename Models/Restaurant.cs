using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZamawianiePosiłkowOnline.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantID { get; set; }
        [DisplayName("Nazwa Restauracji")]
        public string RestaurantName { get; set; }
        [DisplayName("Zdjęcie")]
        public string ImageUrl { get; set; }
    }
}
