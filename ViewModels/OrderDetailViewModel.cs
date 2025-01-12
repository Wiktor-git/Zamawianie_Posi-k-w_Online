using System.ComponentModel.DataAnnotations;
using ZamawianiePosiłkowOnline.Models;

namespace ZamawianiePosiłkowOnline.ViewModels
{
    public class OrderDetailViewModel
    {
        [Key]
        public int ID { get; set; }
        public Order Order { get; set; }
        public List<OrderItem> Orders { get; set; }
    }
}
