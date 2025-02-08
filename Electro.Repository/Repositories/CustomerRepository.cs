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
    public class CustomerRepository : GenericRepository<Customer>
    {
        private readonly ElectroDbContext _dbContext;

        public CustomerRepository(ElectroDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public override async Task<Customer> GetById(object id)
        {
            var customer = await _dbContext.Customers.Where(C => C.Id == id.ToString())
                                                    .Include(C => C.Cart)
                                                    .Include(C => C.Orders)
                                                    .FirstOrDefaultAsync();
            return customer;
        }
        public override async Task<IEnumerable<Customer>> GetAll()
        {
            var customers = await _dbContext.Customers.Include(C => C.Cart)
                                                     .Include(C => C.Orders)
                                                     .ToListAsync();
            return customers;
        }
    }
}
