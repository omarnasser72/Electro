using Electro.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electro.Core.Entities
{
    public class OrderProduct : IEntity
    {
        public Order? Order { get; set; }
        public string CustomerId { get; set; }
        public DateTime Date { get; set; }
        public Product? Product { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
