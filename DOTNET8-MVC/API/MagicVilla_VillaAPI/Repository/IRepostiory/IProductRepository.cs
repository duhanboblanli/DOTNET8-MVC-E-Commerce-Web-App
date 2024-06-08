using API.Models;
using System.Linq.Expressions;

namespace API.Repository.IRepostiory
{
    public interface IProductRepository : IRepository<Product>
    {
      
        Task<Product> UpdateAsync(Product entity);
  
    }
}
