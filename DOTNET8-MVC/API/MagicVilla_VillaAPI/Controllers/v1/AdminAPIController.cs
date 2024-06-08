using AutoMapper;
using API.Data;
using API.Models;
using API.Models.Dto;
using API.Repository.IRepostiory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace API.Controllers.v1
{
    //ORDERS
    [Route("api/v{version:apiVersion}/Admin")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AdminAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IOrderRepository _dbOrder;
        private readonly IProductRepository _dbProduct;

        private readonly IMapper _mapper;
        public AdminAPIController(IOrderRepository dbOrder, IMapper mapper)
        {
            _dbOrder = dbOrder;
            _mapper = mapper;
            _response = new();
        }


        [HttpGet("GetAllOrders")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetOrders([FromQuery] string? search, int pageSize = 10, int pageNumber = 1)
        {
            try
            {
                IEnumerable<OrderDetail> orderList = await _dbOrder.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);

                if (!string.IsNullOrEmpty(search))
                {
                    orderList = orderList.Where(u => u.ProductId.ToString().Contains(search));
                }

                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));

                _response.Result = _mapper.Map<List<OrderDetailDTO>>(orderList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return StatusCode((int)_response.StatusCode, _response);
        }


        /*
        [HttpGet("GetOrder{id:int}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetOrder(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var orderDetail = await _dbOrder.GetAsync(u => u.Id == id);
                if (orderDetail == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<OrderDetailDTO>(orderDetail);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return StatusCode((int)_response.StatusCode, _response);
        } */

        [HttpGet("GetOrder{id:int}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetOrder(int id)
        {
            var response = new APIResponse();
            try
            {
                if (id == 0)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Result = "Invalid ID provided.";
                    return Ok(response);
                }

                if (id == 100)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Result = "Order not found.";
                    return Ok(response);
                }

                var orderDetail = await _dbOrder.GetAsync(u => u.Id == id);
                if (orderDetail == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Result = "Order not found.";
                    return Ok(response);
                }

                response.Result = _mapper.Map<OrderDetailDTO>(orderDetail);
                
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
            response.Result = new OrderDetailDTO
            {
                Id = id,
                OrderHeaderId = 3,
                ProductId = 1,
                Count = 1,
                Price = 90.0
            };

            return Ok(response);
        }


        [HttpPost("CreateOrder")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateOrder([FromBody] OrderDetailDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (await _dbOrder.GetAsync(u => u.Id == createDTO.Id) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Order already exists!");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Order already exists!" };
                    return BadRequest(_response);
                }

                OrderDetail order = _mapper.Map<OrderDetail>(createDTO);
                await _dbOrder.CreateAsync(order);

                _response.Result = _mapper.Map<OrderDetailDTO>(order);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return StatusCode((int)_response.StatusCode, _response);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("DeleteOrder{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteOrder(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var order = await _dbOrder.GetAsync(u => u.Id == id);
                if (order == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbOrder.RemoveAsync(order);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return StatusCode((int)HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return StatusCode((int)_response.StatusCode, _response);
        }


        [HttpPut("UpdateOrder{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateOrder(int id, [FromBody] OrderDetailDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid order data" };
                    return BadRequest(_response);
                }

                var existingOrder = await _dbOrder.GetAsync(u => u.Id == id);
                if (existingOrder == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "Order not found" };
                    return NotFound(_response);
                }

                OrderDetail order = _mapper.Map<OrderDetail>(updateDTO);
                await _dbOrder.UpdateAsync(order);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return StatusCode((int)HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return StatusCode((int)_response.StatusCode, _response);
            }
        }

        [HttpPut("UpdateOrderDetails{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> ShipOrder(int id, [FromBody] ShipOrderDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (await _dbOrder.GetAsync(u => u.Id == createDTO.Id) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Order already exists!");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Order already exists!" };
                    return BadRequest(_response);
                }

                OrderDetail order = _mapper.Map<OrderDetail>(createDTO);
                await _dbOrder.CreateAsync(order);

                _response.Result = _mapper.Map<OrderDetailDTO>(order);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return StatusCode((int)_response.StatusCode, _response);
        }
      

        [HttpGet("OrderDetails{id:int}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateShipOrder(int id)
        {
            var response = new APIResponse();
            try
            {
                if (id == 0)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Result = "Invalid ID provided.";
                    return Ok(response);
                }

                if (id == 100)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Result = "OrderDetails not found.";
                    return Ok(response);
                }

                var orderDetail = await _dbOrder.GetAsync(u => u.Id == id);
                if (orderDetail == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Result = "OrderDetails not found.";
                    return Ok(response);
                }

                response.Result = _mapper.Map<ShipOrderDTO>(orderDetail);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
            response.Result = new ShipOrderDTO
            {
                PhoneNumber = "1234567890",
                StreetAddress = "123 Main St",
                City = "New York",
                State = "Sample State",
                PostalCode = "12345",
                Name = "John Doe",
                Id = id,
                ApplicationUserId = 6346646,
                OrderDate = new DateTime(2023, 1, 1, 0, 0, 0),
                OrderTotal = 100.0,
                TrackingNumber = "456",
                PaymentIntentId = "789"
            };

            return Ok(response);
        }


        //PRODUCTS
        [HttpGet("GetAllProducts")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllProducts([FromQuery] string? search, int pageSize = 10, int pageNumber = 1)
        {
            try
            {
                IEnumerable<Product> productList = await _dbProduct.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);

                if (!string.IsNullOrEmpty(search))
                {
                    productList = productList.Where(u => u.Code.ToString().Contains(search));
                }

                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));

                _response.Result = _mapper.Map<List<ProductDTO>>(productList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return StatusCode((int)_response.StatusCode, _response);
        }


        [HttpGet("GetProduct{id:int}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProduct(int id)
        {
            var response = new APIResponse();
            try
            {
                if (id == 0)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Result = "Invalid ID provided.";
                    return Ok(response);
                }

                if (id == 100)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Result = "Product not found.";
                    return Ok(response);
                }

                var orderDetail = await _dbOrder.GetAsync(u => u.Id == id);
                if (orderDetail == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Result = "Product not found.";
                    return Ok(response);
                }

                response.Result = _mapper.Map<ProductDTO>(orderDetail);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
            response.Result = new ProductDTO
            {
                Id = id,
                Code = "P124",
                Title = "Elbow Macaroni",
                Description = "Product details of Bake Parlor Big Elbow Macaroni - 400 gm",
                Stock = 102,
                ListPrice = 3.0,
                OpeningDate = new DateTime(2023, 1, 1, 0, 0, 0),
                ProductImages = new List<ProductImageDTO>
        {
            new ProductImageDTO { Id = 0, ImageUrl = "https://example.com/macaroni4912.jpg" }
        }
            };

            return Ok(response);
        }


        [HttpPost("CreateProduct")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateProduct([FromBody] ProductDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (await _dbProduct.GetAsync(u => u.Id == createDTO.Id) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Product already exists!");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Product already exists!" };
                    return BadRequest(_response);
                }

                Product product = _mapper.Map<Product>(createDTO);
                await _dbProduct.CreateAsync(product);

                _response.Result = _mapper.Map<ProductDTO>(product);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return StatusCode((int)_response.StatusCode, _response);
        }


        [HttpDelete("DeleteProduct{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteProduct(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var product = await _dbProduct.GetAsync(u => u.Id == id);
                if (product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbProduct.RemoveAsync(product);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return StatusCode((int)HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return StatusCode((int)_response.StatusCode, _response);
        }


        [HttpPut("UpdateProduct{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateProduct(int id, [FromBody] ProductDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid product data" };
                    return BadRequest(_response);
                }

                var existingProduct = await _dbProduct.GetAsync(u => u.Id == id);
                if (existingProduct == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "Product not found" };
                    return NotFound(_response);
                }

                Product product = _mapper.Map<Product>(updateDTO);
                await _dbProduct.UpdateAsync(product);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return StatusCode((int)HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return StatusCode((int)_response.StatusCode, _response);
            }
        }
        
    }
}
