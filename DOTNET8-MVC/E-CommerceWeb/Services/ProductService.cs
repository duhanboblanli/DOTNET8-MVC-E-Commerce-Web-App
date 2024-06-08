using Utility;
using API.Models;
using API.Models.Dto;
using ECommerceWeb.Services.IServices;
using Newtonsoft.Json.Linq;
using ECommerceWeb.Services;

namespace ECommerceWeb.Services
{
    public class ProductService : BaseService, Services.IServices.ProductService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string productUrl;

        public ProductService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            productUrl = configuration.GetValue<string>("ServiceUrls:API");

        }

        public Task<T> CreateAsync<T>(ProductDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD1.ApiType.POST,
                Data = dto,
                Url = productUrl + "/api/v1/Admin/",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD1.ApiType.DELETE,
                Url = productUrl + "/api/v1/Admin/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD1.ApiType.GET,
                Url = productUrl + "/api/v1/Admin/",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD1.ApiType.GET,
                Url = productUrl + "/api/v1/Admin/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(ProductDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD1.ApiType.PUT,
                Data = dto,
                Url = productUrl + "/api/v1/Admin/" + dto.Code,
                Token = token
            }) ;
        }

        Task Services.IServices.ProductService.CreateAsync<T>(ProductDTO model, string v)
        {
            throw new NotImplementedException();
        }

        Task Services.IServices.ProductService.UpdateAsync<T>(ProductDTO model, string v)
        {
            throw new NotImplementedException();
        }
    }
}
