using API.Data;
using API.Models;
using API.Repository.IRepostiory;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Repository
{
    public class OrderRepository : Repository<OrderDetail>, IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

  
        public async Task<OrderDetail> UpdateAsync(OrderDetail entity)
        { 
            _db.Orders.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
