using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ShopEasy.Ims.Domain.Models.DbModels;

namespace ShopEasy.Ims.Domain.Reports
{
    public class ProductStockReport : IDocument
    {
        private readonly string _title;
        private readonly List<Product> _products;

        public ProductStockReport(string title, List<Product> products)
        {
            _title = title;
            _products = products;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

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

                page.Content().PaddingVertical(0.3f, Unit.Centimetre).Element(ComposeTable);

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        }

        void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Item Name").FontSize(12).SemiBold();
                    header.Cell().Element(CellStyle).Text("Quantity").FontSize(12).SemiBold();
                    header.Cell().Element(CellStyle).Text("Minimum Stcok").FontSize(12).SemiBold();
                    header.Cell().Element(CellStyle).Text("Price").FontSize(12).SemiBold();
                    header.Cell().Element(CellStyle).Text("Stock Status").FontSize(12).SemiBold();

                    static IContainer CellStyle(IContainer container)
                    {
                        return container
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Medium)
                            .Padding(5)
                            .AlignCenter();
                    }
                });

                foreach (var item in _products)
                {
                    table.Cell().Element(CellStyle).Text(item.Name);
                    table.Cell().Element(CellStyle).Text(item.QuantityInStock.ToString());
                    table.Cell().Element(CellStyle).Text(item.MinimumStock.ToString());
                    table.Cell().Element(CellStyle).Text($"LKR {item.Price:F2}");
                    table.Cell().Element(CellStyle).Text(item.QuantityInStock == 0 ? "Out of stock" : (item.QuantityInStock > item.MinimumStock ? "Available in stock" : "Low in stock"));

                    static IContainer CellStyle(IContainer container)
                    {
                        return container
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .Padding(5)
                            .AlignCenter();
                    }
                }
            });
        }

        public void GeneratePdf(Stream stream)
        {
            Document.Create(container => Compose(container)).GeneratePdf(stream);
        }
    }
}
