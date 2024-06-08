using API.Models.Dto;

namespace ECommerceWeb.Services.IServices
{
    public interface ProductService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        //Task<T> CreateAsync<T>(VillaNumberCreateDTO dto, string token);
        //Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
        Task CreateAsync<T>(ProductDTO model, string v);
        Task UpdateAsync<T>(ProductDTO model, string v);
    }
}
