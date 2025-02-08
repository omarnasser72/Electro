using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electro.Core.Entities
{
    public class Customer : User
    {
        public Cart? Cart { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
