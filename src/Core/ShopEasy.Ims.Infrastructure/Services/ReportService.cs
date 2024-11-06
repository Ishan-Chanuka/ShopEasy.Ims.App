using ShopEasy.Ims.Application.Services;
using ShopEasy.Ims.Domain.Models.DbModels;
using ShopEasy.Ims.Domain.Models.ResponseModels;
using ShopEasy.Ims.Domain.Reports;
using ShopEasy.Ims.Infrastructure.Context;

namespace ShopEasy.Ims.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        #region Private Fields
        private readonly ApplicationDbContext _context;
        #endregion

        #region Constructor
        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Private Methods
        private IQueryable<Product> GetProducts()
        {
            return _context.Products.Where(p => p.IsDeleted == false);
        }
        #endregion

        public Task<byte[]> GenerateProductStockReportAsync(int type)
        {
            if (type != 1 && type != 2)
            {
                return Task.FromResult(new byte[0]);
            }

            // type 1: Low stock
            // type 2: Out of stock
            var products = type switch
            {
                1 => GetProducts().Where(p => p.QuantityInStock < p.MinimumStock).OrderBy(p => p.Name).ToList(),
                2 => GetProducts().Where(p => p.QuantityInStock == 0).OrderBy(p => p.Name).ToList(),
                _ => throw new NotImplementedException()
            };

            string title = type switch
            {
                1 => "Low Stock Report",
                2 => "Out of Stock Report",
                _ => throw new NotImplementedException()
            };

            var document = new ProductStockReport(title, products);

            using Stream stream = new MemoryStream();
            document.GeneratePdf(stream);

            return Task.FromResult(((MemoryStream)stream).ToArray());
        }

        public Task<byte[]> GenerateProductSummaryReportAsync()
        {
            var products = GetProducts().OrderBy(p => p.Name).Select(p => new StockSummaryResponseModel
            {
                TotalItemsInStock = GetProducts().Count(),
                TotalValueOfItems = GetProducts().Sum(p => p.QuantityInStock * p.Price),
                StockItems = GetProducts().Select(p => new StockItemResponseModel
                {
                    ProductName = p.Name,
                    Price = p.Price,
                    QuantityInStock = p.QuantityInStock,
                    MinimumStock = p.MinimumStock
                }).ToList()
            }).FirstOrDefault();

            var document = new StockSummaryReport(products);

            using Stream stream = new MemoryStream();
            document.GeneratePdf(stream);

            return Task.FromResult(((MemoryStream)stream).ToArray());
        }
    }
}
