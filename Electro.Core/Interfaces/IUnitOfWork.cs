//using Electro.Core.Entities;
//using Electro.Core.Interfaces.Repositories;
//using Electro.Repository.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Electro.Core.Interfaces
//{
//    public interface IUnitOfWork<T> where T : class, IEntity
//    {
//        //Signature for property for each repository interface
//        public IGenericRepository<T> GenericRepository { get; set; }
//        public CustomerRepository CustomerRepository { get; set; }
//        public ProductRepository ProductRepository { get; set; }
//        public CartRepository CartRepository { get; set; }
//        public OrderRepository OrderRepository { get; set; }
//        public CategoryRepository CategoryRepository { get; set; }
//        public Task<bool> Complete();
//        public void Dispose();
//    }
//}
