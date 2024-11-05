using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ShopEasy.Ims.Domain.Models.DbModels;

namespace ShopEasy.Ims.Domain.Reports
{
    public class ProductSummaryReport
    {
        private readonly List<Product> _products;
        private const string _title = "Product Summary Report";

        public ProductSummaryReport(List<Product> products)
        {
            _products = products;
        }

        public void Compose(IDocumentContainer container)
        {
            container
            .Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(0.5f, Unit.Inch);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text(_title)
                    .SemiBold()
                    .FontSize(20)
                    .FontColor(Colors.Blue.Medium);


                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        }

        public void GeneratePdf(Stream stream)
        {
            Document.Create(container => Compose(container)).GeneratePdf(stream);
        }
    }
}
