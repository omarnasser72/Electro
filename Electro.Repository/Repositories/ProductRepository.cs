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
    public class ProductRepository : GenericRepository<Product>
    {
        private readonly ElectroDbContext _dbContext;

        public ProductRepository(ElectroDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public override async Task<Product> GetById(object id)
        {
            //var product = await DbContext.Set<Product>().FindAsync(id);
            var product = await _dbContext.Products.Where(P => P.Id.ToString() == id.ToString())
                                                  .Include(P => P.Category)
                                                  .Include(P => P.Brand)
                                                  .FirstOrDefaultAsync();
            return product;
        }
        public override async Task<IEnumerable<Product>> GetAll()
        {
            var products = await _dbContext.Products.Include(P => P.Category)
                                                   .Include(P => P.Brand)
                                                   .ToListAsync();
            return products;
        }
        public async Task<Product?> GetProductByNameAsync(Product product)
            => await _dbContext.Products.Where(P => P.Name == product.Name).FirstOrDefaultAsync();

        public async Task<IEnumerable<Product>> GetProductsByCategoryName(string CategoryName)
            => await _dbContext.Products.Include(P => P.Category)
                                       .Where(P => P.Category.Name == CategoryName)
                                       .ToListAsync();

        public async Task<IEnumerable<Product>> GetProductsByBrandName(string BrandName)
            => await _dbContext.Products.Include(P => P.Brand)
                                       .Where(P => P.Brand.Name == BrandName)
                                       .ToListAsync();

    }
}
