using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ShopEasy.Ims.Domain.Models.ResponseModels;

namespace ShopEasy.Ims.Domain.Reports
{
    public class StockSummaryReport : IDocument
    {
        private readonly StockSummaryResponseModel _products;
        private const string _title = "Stock Summary Report";

        public StockSummaryReport(StockSummaryResponseModel products)
        {
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
                page.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));

                page.Header()
                    .Text(_title)
                    .SemiBold()
                    .FontSize(20)
                    .FontColor(Colors.Blue.Medium);

                page.Content().Column(column =>
                {
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Text($"Date: {DateTime.Now:d}");
                    });

                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Text($"Total Items In Stock: {_products.TotalItemsInStock}");
                    });

                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Text($"Total Value Of Items: {_products.TotalValueOfItems:F2}");
                    });

                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Text($"Total Stock Quantity: {_products.StockItems.Sum(p => p.QuantityInStock)}");
                    });

                    column.Item().PaddingVertical(10);

                    column.Item().Element(ComposeTable);
                });

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
                    columns.RelativeColumn(2);  // Product Name
                    columns.RelativeColumn();   // Quantity
                    columns.RelativeColumn();   // Price
                    columns.RelativeColumn();   // Stock status
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Product Name").FontSize(12).SemiBold();
                    header.Cell().Element(CellStyle).Text("Quantity").FontSize(12).SemiBold();
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

                foreach (var item in _products.StockItems)
                {
                    table.Cell().Element(CellStyle).Text(item.ProductName);
                    table.Cell().Element(CellStyle).Text(item.QuantityInStock.ToString());
                    table.Cell().Element(CellStyle).Text($"LKR {item.Price.ToString()}");
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

                static IContainer TotalCellStyle(IContainer container)
                {
                    return container
                        .Padding(5)
                        .AlignCenter();
                }
            });
        }

        public void GeneratePdf(Stream stream)
        {
            Document.Create(container => Compose(container)).GeneratePdf(stream);
        }
    }
}
