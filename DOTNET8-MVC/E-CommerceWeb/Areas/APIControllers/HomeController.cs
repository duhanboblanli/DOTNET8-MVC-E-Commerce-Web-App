using ECommerceWeb.Services.IServices;
using Utility;
using API.Models;
using API.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ECommerceWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductService _productService;
       // private readonly IMapper _mapper;
        public HomeController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDTO> list = new();

            var response = await _productService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD1.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
       
    }
}