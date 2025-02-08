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
    public class CategoryRepository : GenericRepository<Category>
    {
        private readonly ElectroDbContext _dbContext;

        public CategoryRepository(ElectroDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Category?> GetCategoryByNameAsync(Category category)
            => await _dbContext.Categories.Where(C => C.Name == category.Name).FirstOrDefaultAsync();


    }
}
