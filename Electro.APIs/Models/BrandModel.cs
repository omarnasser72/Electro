using System.ComponentModel.DataAnnotations;

namespace Electro.APIs.Models
{
    public class BrandModel
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }
    }
}
