using Microsoft.AspNetCore.Mvc;
using ShopEasy.Ims.Application.Services;

namespace ShopEasy.Ims.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("product-stock-report/{type}")]
        public async Task<IActionResult> GenerateProductStockReport(int type)
        {
            var report = await _reportService.GenerateProductStockReportAsync(type);

            var fileName = "ProductStockReport.pdf";

            return File(new MemoryStream(report), "application/pdf", fileName);
        }
    }
}
