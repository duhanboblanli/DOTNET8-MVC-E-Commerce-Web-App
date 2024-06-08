using Utility;
using API.Models;
using API.Models.Dto;
using ECommerceWeb.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace ECommerceWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _productService;
     
        public OrderController(OrderService orderService)
        {
            _productService = orderService;
         
        }

        public async Task<IActionResult> IndexOrder()
        {
            List<OrderDetailDTO> list = new();

            var response = await _productService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD1.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<OrderDetailDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        
        public async Task<IActionResult> CreateOrder()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(ProductDTO model, int productID)
        {
            if (ModelState.IsValid)
            {

                var response = await _productService.GetAsync<APIResponse>(productID, HttpContext.Session.GetString(SD1.SessionToken));
                if (response != null && response.IsSuccess)
{
                    TempData["success"] = "Order created successfully";
                    return RedirectToAction(nameof(IndexOrder));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateOrder(int productID)
{
            var response = await _productService.GetAsync<APIResponse>(productID, HttpContext.Session.GetString(SD1.SessionToken));
            if (response != null && response.IsSuccess)
            {

                ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View();
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrder(ProductDTO model, int productID)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Product updated successfully";
                var response = await _productService.GetAsync<APIResponse>(productID, HttpContext.Session.GetString(SD1.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexOrder));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteOrder(int villaId)
        {
            var response = await _productService.GetAsync<APIResponse>(villaId, HttpContext.Session.GetString(SD1.SessionToken));
            if (response != null && response.IsSuccess)
            {
                ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(ProductDTO model)
        {
            
                var response = await _productService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD1.SessionToken));
                if (response != null && response.IsSuccess)
                {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(IndexOrder));
                }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

    }
}
