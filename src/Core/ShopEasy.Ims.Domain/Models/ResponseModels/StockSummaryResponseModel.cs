namespace ShopEasy.Ims.Domain.Models.ResponseModels
{
    public class StockSummaryResponseModel
    {
        public int TotalItemsInStock { get; set; }
        public decimal TotalValueOfItems { get; set; }
        public List<StockItemResponseModel> StockItems { get; set; }
    }
}
