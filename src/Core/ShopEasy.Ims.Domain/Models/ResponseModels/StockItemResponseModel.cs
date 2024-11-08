namespace ShopEasy.Ims.Domain.Models.ResponseModels
{
    public class StockItemResponseModel
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int MinimumStock { get; set; }
    }
}
