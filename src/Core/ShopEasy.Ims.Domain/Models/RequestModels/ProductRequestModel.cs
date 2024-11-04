namespace ShopEasy.Ims.Domain.Models.RequestModels
{
    public class ProductRequestModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int MinimumStock { get; set; }
        public int UserId { get; set; }
    }
}
