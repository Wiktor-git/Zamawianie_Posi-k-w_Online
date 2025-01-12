using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ZamawianiePosiłkowOnline.Models;

namespace ZamawianiePosiłkowOnline.ViewModels
{
    [Keyless]
    public class MealCreateHelperViewModel
    {
        public Meal Meal { get; set; }
        public List<Restaurant> Restaurants { get; set; }
    }
}
