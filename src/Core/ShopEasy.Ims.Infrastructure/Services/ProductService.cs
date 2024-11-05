using Microsoft.EntityFrameworkCore;
using ShopEasy.Ims.Application.Services;
using ShopEasy.Ims.Domain.Models.DbModels;
using ShopEasy.Ims.Domain.Models.RequestModels;
using ShopEasy.Ims.Domain.Models.ResponseModels;
using ShopEasy.Ims.Domain.Primitives.ApiResponse;
using ShopEasy.Ims.Infrastructure.Context;
using System.Net;

namespace ShopEasy.Ims.Infrastructure.Services
{
    public class ProductService : IProductsService
    {
        #region Private Fields
        private readonly ApplicationDbContext _context;
        #endregion

        #region Constructor
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        public async Task<ApiResponse<int>> AddProductAsync(ProductRequestModel request)
        {
            if (request == null)
            {
                return new ApiResponse<int>((int)HttpStatusCode.BadRequest, "Request cannot be null", false);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var product = new Product
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    QuantityInStock = request.QuantityInStock,
                    MinimumStock = request.MinimumStock,
                    IsStockLow = (request.QuantityInStock <= request.MinimumStock ? true : false),
                    CreatedBy = request.UserId
                };

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ApiResponse<int>()
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Message = "Product added successfully",
                    IsSuccess = true,
                    Data = product.ProductId
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new ApiResponse<int>((int)HttpStatusCode.InternalServerError, "An error occurred while adding product", false);
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }

        public async Task<ApiResponse<bool>> DeleteProductAsync(int id, int userId)
        {
            if (id <= 0)
            {
                return new ApiResponse<bool>((int)HttpStatusCode.BadRequest, "Invalid product id", false);
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return new ApiResponse<bool>((int)HttpStatusCode.NotFound, "Product not found", false);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                product.IsDeleted = true;
                product.DeletedAt = DateTime.Now;
                product.DeletedBy = userId;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ApiResponse<bool>()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "Product deleted successfully",
                    IsSuccess = true,
                    Data = true
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new ApiResponse<bool>((int)HttpStatusCode.InternalServerError, "An error occurred while deleting product", false);
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }

        public async Task<ApiResponse<IEnumerable<ProductResponseModel>>> GetProductAsync()
        {
            var products = await _context.Products
                                         .Where(p => !p.IsDeleted)
                                         .Select(p => new ProductResponseModel
                                         {
                                             ProductId = p.ProductId,
                                             Name = p.Name,
                                             Description = p.Description,
                                             Price = p.Price,
                                             QuantityInStock = p.QuantityInStock,
                                             MinimumStock = p.MinimumStock,
                                             StockStatus = p.QuantityInStock == 0 ? "Out of Stock" : (p.IsStockLow ? "Low" : "Available")
                                         }).ToListAsync();

            return new ApiResponse<IEnumerable<ProductResponseModel>>()
            {
                StatusCode = products != null && products.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound,
                Message = products != null && products.Any() ? "Products retrieved successfully" : "No products found",
                IsSuccess = products != null && products.Any(),
                Data = products
            };
        }

        public async Task<ApiResponse<ProductResponseModel>> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                                 .Where(p => p.ProductId == id)
                                 .Select(p => new ProductResponseModel
                                 {
                                     ProductId = p.ProductId,
                                     Name = p.Name,
                                     Description = p.Description,
                                     Price = p.Price,
                                     QuantityInStock = p.QuantityInStock,
                                     MinimumStock = p.MinimumStock,
                                     StockStatus = p.QuantityInStock == 0 ? "Out of Stock" : (p.IsStockLow ? "Low" : "Available")
                                 }).FirstOrDefaultAsync();

            return new ApiResponse<ProductResponseModel>()
            {
                StatusCode = product != null ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound,
                Message = product != null ? "Product retrieved successfully" : "Product not found",
                IsSuccess = product != null,
                Data = product
            };
        }

        public async Task<ApiResponse<bool>> UpdateProductAsync(int productId, ProductRequestModel request)
        {
            if (request == null)
            {
                return new ApiResponse<bool>((int)HttpStatusCode.BadRequest, "Request cannot be null", false);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var product = await _context.Products.FindAsync(productId);

                if (product == null)
                {
                    return new ApiResponse<bool>((int)HttpStatusCode.NotFound, "Product not found", false);
                }

                product.Name = request.Name;
                product.Description = request.Description;
                product.Price = request.Price;
                product.QuantityInStock = request.QuantityInStock;
                product.MinimumStock = request.MinimumStock;
                product.UpdatedAt = DateTime.Now;
                product.UpdatedBy = request.UserId;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ApiResponse<bool>()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "Product updated successfully",
                    IsSuccess = true,
                    Data = true
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new ApiResponse<bool>((int)HttpStatusCode.InternalServerError, "An error occurred while updating product", false);
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }
    }
}
