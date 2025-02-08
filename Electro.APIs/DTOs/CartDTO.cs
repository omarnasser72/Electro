namespace Electro.APIs.DTOs
{
    public class CartDTO
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public ICollection<CartProductDTO> CartProducts { get; set; }
    }
}
