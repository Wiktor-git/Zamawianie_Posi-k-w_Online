using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace ZamawianiePosiłkowOnline.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ScheduledDeliveryDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        [Column(TypeName = "money")]
        public decimal TotalPaid { get; set; }
        [Column(TypeName = "money")]
        public decimal DiscountAmmount { get; set; }
    }
}