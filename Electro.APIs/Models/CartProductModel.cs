using System.ComponentModel.DataAnnotations;

namespace Electro.APIs.Models
{
    public class CartProductModel
    {
        //[Required]
        //public string CartId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

    }
}
