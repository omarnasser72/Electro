using System.ComponentModel.DataAnnotations;

namespace Electro.APIs.Models
{
    public class CustomerModel
    {
        [Required]
        public string Id { get; set; }

    }
}
