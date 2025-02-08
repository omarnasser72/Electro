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
    public class CartRepository : GenericRepository<Cart>
    {
        private readonly ElectroDbContext _dbContext;

        public CartRepository(ElectroDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public override Task<Cart?> GetById(object id)
        {
            var cart = _dbContext.Carts.Where(C => C.CustomerId == id.ToString())
                                      .Include(C => C.Customer)
                                      .Include(C => C.CartProducts!)
                                            .ThenInclude(CP => CP.Product)
                                                .ThenInclude(P => P.Brand)
                                      .Include(C => C.CartProducts!)
                                            .ThenInclude(CP => CP.Product)
                                                .ThenInclude(P => P.Category)
                                      .FirstOrDefaultAsync();

            return cart;
        }
        public override async Task<IEnumerable<Cart>> GetAll()
        {
            var carts = await _dbContext.Carts.Include(C => C.Customer)
                                              .Include(C => C.Customer)
                                              .Include(C => C.CartProducts!)
                                                    .ThenInclude(CP => CP.Product)
                                                        .ThenInclude(P => P.Brand)
                                              .Include(C => C.CartProducts!)
                                                    .ThenInclude(CP => CP.Product)
                                                        .ThenInclude(P => P.Category).ToListAsync();
            return carts;
        }

    }
}
