using System.ComponentModel.DataAnnotations;

namespace Electro.APIs.Models
{
    public class CategoryModel
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
    }
}
