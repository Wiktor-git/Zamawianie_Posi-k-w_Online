namespace ZamawianiePosiłkowOnline.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public Meal CartMeal { get; set; }
        public int Quantity { get; set; }
    }
}
