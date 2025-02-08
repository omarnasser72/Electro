using Electro.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electro.Core.Entities
{
    public class Cart : IEntity
    {
        public string CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<CartProduct>? CartProducts { get; set; }
    }
}
