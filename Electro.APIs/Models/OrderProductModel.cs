using Electro.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Electro.APIs.Models
{
    public class OrderProductModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
