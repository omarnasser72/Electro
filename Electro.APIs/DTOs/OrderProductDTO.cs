using Electro.Core.Entities;

namespace Electro.APIs.DTOs
{
    public class OrderProductDTO
    {
        public ProductDTO? Product { get; set; }
        public int Quantity { get; set; }
    }
}
