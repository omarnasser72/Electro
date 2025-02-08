using Electro.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electro.Core.Entities
{
    public class Order : IEntity
    {
        public string CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public DateTime DateTime { get; set; }
        public bool? Payed { get; set; }
        public ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}

