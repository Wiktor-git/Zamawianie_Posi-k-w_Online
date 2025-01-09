using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZamawianiePosiłkowOnline.Models
{
    public class City
    {
        [Key]
        public int CityID { get; set; }
        [DisplayName("Miasto")]
        public string CityName { get; set; }
        [DisplayName("Zdjęcie")]
        public string ImageUrl { get; set; }
    }
}
