namespace ShopEasy.Ims.Application.Services
{
    public interface IReportService
    {
        Task<byte[]> GenerateProductStockReportAsync(int type);
        Task<byte[]> GenerateProductSummaryReportAsync();
    }
}
