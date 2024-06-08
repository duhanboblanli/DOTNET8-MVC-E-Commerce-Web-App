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
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
     
        public ProductController(ProductService productService)
        {
            _productService = productService;
         
        }

        public async Task<IActionResult> IndexProduct()
        {
            List<ProductDTO> list = new();

            var response = await _productService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD1.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> CreateProduct()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductDTO model, int productID)
        {
            if (ModelState.IsValid)
            {

                var response = await _productService.GetAsync<APIResponse>(productID, HttpContext.Session.GetString(SD1.SessionToken));
                if (response != null && response.IsSuccess)
{
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(IndexProduct));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct(int productID)
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
        public async Task<IActionResult> UpdateProduct(ProductDTO model, int productID)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Product updated successfully";
                var response = await _productService.GetAsync<APIResponse>(productID, HttpContext.Session.GetString(SD1.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexProduct));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int villaId)
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
        public async Task<IActionResult> DeleteProduct(ProductDTO model)
        {
            
                var response = await _productService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD1.SessionToken));
                if (response != null && response.IsSuccess)
                {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(IndexProduct));
                }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

    }
}
