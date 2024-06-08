using Utility;
using API.Models;
using API.Models.Dto;
using ECommerceWeb.Services.IServices;
using ECommerceWeb.Services;

namespace ECommerceWeb.Services
{
    public class OrderService : BaseService, ECommerceWeb.Services.IServices.OrderService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string orderUrl;

        public OrderService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            orderUrl = configuration.GetValue<string>("ServiceUrls:API");

        }

        public Task<T> CreateAsync<T>(OrderDetailDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD1.ApiType.POST,
                Data = dto,
                Url = orderUrl + "/api/v1/Customer/",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD1.ApiType.DELETE,
                Url = orderUrl + "/api/v1/Customer/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD1.ApiType.GET,
                Url = orderUrl + "/api/v1/Customer/",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD1.ApiType.GET,
                Url = orderUrl + "/api/v1/Customer/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(OrderDetailDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD1.ApiType.PUT,
                Data = dto,
                Url = orderUrl + "/api/v1/villaAPI/" + dto.Id,
                Token = token
            }) ;
        }
    }
}
