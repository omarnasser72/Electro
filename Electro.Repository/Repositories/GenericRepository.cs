using Electro.Core.Entities;
using Electro.Core.Interfaces.Repositories;
using Electro.Repository.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electro.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly ElectroDbContext _dbContext;

        public GenericRepository(ElectroDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public virtual void Delete(object id)
        {
            var entity = _dbContext.Set<T>().Find(id);
            if (entity != null)
                _dbContext.Set<T>().Remove(entity);
        }
        public virtual void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public virtual async Task<IEnumerable<T>> GetAll()
            => await _dbContext.Set<T>().ToListAsync();

        public virtual async Task<T?> GetById(object id)
            => await _dbContext.Set<T>().FindAsync(id);

    }
}
