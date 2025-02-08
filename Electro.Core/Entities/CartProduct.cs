using Electro.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electro.Core.Entities
{
    public class CartProduct : IEntity
    {
        public Cart? Cart { get; set; }
        public string CartId { get; set; }

        public Product? Product { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
