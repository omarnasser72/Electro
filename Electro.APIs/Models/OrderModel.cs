using Electro.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Electro.APIs.Models
{
    public class OrderModel
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public bool? Payed { get; set; }

        [Required]
        public ICollection<OrderProductModel>? OrderProducts { get; set; }
    }
}
