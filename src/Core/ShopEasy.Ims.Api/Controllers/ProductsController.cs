using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopEasy.Ims.Application.Services;
using ShopEasy.Ims.Domain.Models.RequestModels;
using ShopEasy.Ims.Domain.Models.ResponseModels;
using ShopEasy.Ims.Domain.Primitives.ApiResponse;

namespace ShopEasy.Ims.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet("")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductResponseModel>>>> GetProducts()
        {
            var products = await _productsService.GetProductAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductResponseModel>>> GetProduct(int id)
        {
            var product = await _productsService.GetProductByIdAsync(id);
            return Ok(product);
        }


        [HttpPost("")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ProductResponseModel>>> CreateProduct(ProductRequestModel product)
        {
            var createdProduct = await _productsService.AddProductAsync(product);
            return Ok(createdProduct);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ProductResponseModel>>> UpdateProduct(int id, ProductRequestModel product)
        {
            var updatedProduct = await _productsService.UpdateProductAsync(id, product);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ProductResponseModel>>> DeleteProduct(int id)
        {
            var deletedProduct = await _productsService.DeleteProductAsync(id, 1);
            return Ok(deletedProduct);
        }
    }
}
