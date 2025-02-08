using Electro.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electro.Core.Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(5)]
        public string Description { get; set; }

        public int QuantityInStock { get; set; }
        public decimal Price { get; set; }

        public Category? Category { get; set; }
        public int CategoryId { get; set; }


        public Brand? Brand { get; set; }
        public int BrandId { get; set; }

        public ICollection<CartProduct>? CartProducts { get; set; }
        public ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
