using API.Models;
using System.Linq.Expressions;

namespace API.Repository.IRepostiory
{
    public interface IOrderRepository : IRepository<OrderDetail>
    {
      
        Task<OrderDetail> UpdateAsync(OrderDetail entity);
  
    }
}
