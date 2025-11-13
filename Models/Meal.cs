using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZamawianiePosiłkowOnline.Models
{
    public class Meal
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Nazwa Dania")]
        [Required(ErrorMessage = "Wymagana poprawna nazwa")]
        [StringLength(50, ErrorMessage = "maks 50 znaków")]
        public string Name { get; set; }
        [Display(Name = "Opis  Dania")]
        [Required(ErrorMessage = "Wymagany poprawny opis")]
        [StringLength(200, ErrorMessage = "maks 200 znaków")]
        public string Description { get; set; }
        [Display(Name = "W jakiej restauracji dostępne")]
        [Required(ErrorMessage = "Wymagane poprawna restauracja")]
        public string CityAvailability { get; set; }
        [Display(Name = "Ilość kalorii")]
        [Required(ErrorMessage = "Wymagana poprawna ilość kalorii")]
        [Range(1, 6000, ErrorMessage = "proszę wpisać poprawną wartość")]
        public int Calories { get; set; }
        [Display(Name = "Waga (gramy)")]
        [Range(1, 6000, ErrorMessage = "proszę wpisać poprawną wartość")]
        [Required(ErrorMessage = "Wymagana poprawna waga")]
        public int Weight { get; set; }
        [Display(Name = "Zdjęcie Dania (190x190)")]
        [Required(ErrorMessage = "Wymagane załączenie zdjęcia")]
        public string ImageUrl {  get; set; }
        [Display(Name = "Koszt Dania")]
        [Column(TypeName = "money")]
        [Range(1, 300, ErrorMessage = "proszę wpisać poprawną wartość")]
        [Required(ErrorMessage = "Wymagana poprawna cena")]
        public decimal MealPrice { get; set; }
    }
}