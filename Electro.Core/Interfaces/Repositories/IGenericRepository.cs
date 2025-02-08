using Electro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electro.Core.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : IEntity
    {
        public Task<T>? GetById(object id);
        public Task<IEnumerable<T>> GetAll();
        public Task AddAsync(T entity);
        public void Update(T entity);
        public void Delete(object id);
    }
}
