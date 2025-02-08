namespace Electro.APIs.Models
{
    public class CartModel
    {
        public string CustomerId { get; set; }
        public ICollection<CartProductModel> CartProducts { get; set; }
    }
}
