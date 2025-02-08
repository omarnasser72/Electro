using Electro.APIs.Models;
using Electro.Core.Entities;

namespace Electro.APIs.DTOs
{
    public class OrderDTO
    {
        public CustomerDTO Customer { get; set; }
        public DateTime DateTime { get; set; }
        public bool? Payed { get; set; }
        public ICollection<OrderProductDTO>? OrderProducts { get; set; }
    }
}
