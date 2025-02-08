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
    public class BrandRepository : GenericRepository<Brand>
    {
        private readonly ElectroDbContext _dbContext;

        public BrandRepository(ElectroDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Brand?> GetBrandByNameAsync(Brand brand)
            => await _dbContext.Brands.Where(B => B.Name == brand.Name).FirstOrDefaultAsync();


    }
}
