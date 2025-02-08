using Electro.Core.Entities;
using Electro.Repository.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electro.Repository.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {
        private readonly ElectroDbContext _dbContext;

        public OrderRepository(ElectroDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Order> GetOrder(string CustomerId, DateTime Date)
        {
            var order = await _dbContext.Orders.Where(O => O.CustomerId == CustomerId && O.DateTime.Equals(Date))
                                              .Include(O => O.Customer)
                                              .Include(O => O.OrderProducts!)
                                                    .ThenInclude(OP => OP.Product)
                                              .FirstOrDefaultAsync();
            return order;
        }

        public override async Task<IEnumerable<Order>> GetAll()
        {
            var orders = await _dbContext.Orders.Include(O => O.Customer)
                                                .Include(O => O.OrderProducts!)
                                                .ThenInclude(OP => OP.Product)
                                                .ToListAsync();

            return orders;
        }

        public async Task Delete(string id, DateTime date)
        {
            var order = await _dbContext.Orders.Where(O => O.CustomerId == id && O.DateTime.Equals(date))
                                               .FirstOrDefaultAsync();
            if (order != null)
                _dbContext.Orders.Remove(order);
        }
    }
}
