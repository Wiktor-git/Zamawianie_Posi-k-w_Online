using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace ZamawianiePosiłkowOnline.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public int MealID { get; set; }
        [Required]  
        public int AmmountOrdered { get;  set; }
        [Column(TypeName = "money")]
        public decimal OrderPrice { get; set; }
    }
}