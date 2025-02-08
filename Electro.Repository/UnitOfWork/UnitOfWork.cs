using Electro.Core.Entities;
using Electro.Core.Interfaces;
using Electro.Core.Interfaces.Repositories;
using Electro.Repository.Contexts;
using Electro.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electro.Repository.UnitOfWork
{
    public class UnitOfWork<T> where T : class, IEntity
    {
        private readonly ElectroDbContext _dbContext;

        public IGenericRepository<T> GenericRepository { get; private set; }
        public CustomerRepository CustomerRepository { get; private set; }
        public ProductRepository ProductRepository { get; private set; }
        public CartRepository CartRepository { get; private set; }
        public OrderRepository OrderRepository { get; private set; }
        public CategoryRepository CategoryRepository { get; private set; }
        public BrandRepository BrandRepository { get; private set; }
        public UnitOfWork(ElectroDbContext dbContext)
        {
            _dbContext = dbContext;

            // Initialize repositories
            GenericRepository = new GenericRepository<T>(_dbContext);
            CustomerRepository = new CustomerRepository(_dbContext);
            ProductRepository = new ProductRepository(_dbContext);
            CartRepository = new CartRepository(_dbContext);
            OrderRepository = new OrderRepository(_dbContext);
            CategoryRepository = new CategoryRepository(_dbContext);
            BrandRepository = new BrandRepository(_dbContext);
        }

        public async Task<bool> Complete()
            => await _dbContext.SaveChangesAsync() > 0;

        public void Dispose()
            => _dbContext.Dispose();
    }
}
