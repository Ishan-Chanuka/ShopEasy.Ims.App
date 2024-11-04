namespace ShopEasy.Ims.Domain.Models.ResponseModels
{
    public class ProductResponseModel
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int MinimumStock { get; set; }
        public required string StockStatus { get; set; }
    }
}
