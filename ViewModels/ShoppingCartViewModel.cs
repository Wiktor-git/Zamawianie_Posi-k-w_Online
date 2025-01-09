using System.ComponentModel.DataAnnotations.Schema;
using ZamawianiePosiłkowOnline.Models;

namespace ZamawianiePosiłkowOnline.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<ShoppingCartItem> CartItems { get; set; }
        [Column(TypeName = "money")]
        public decimal? TotalPrice { get; set; }
        [Column(TypeName = "money")]
        public decimal? TotalDeliveryCost { get; set; }
        [Column(TypeName = "money")]
        public decimal? TotalTotal { get; set; }
        public decimal? TotalQuantity { get; set; }
    }
}
