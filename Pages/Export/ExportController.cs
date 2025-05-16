using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using Rotativa.AspNetCore;
using System.Data;
using ExportApp.Repositories;
using ExportApp.Models;

namespace ExportApp.Pages
{
    public class ExportController : Controller
    {
        private readonly IProductRepository _repo;

        public ExportController(IProductRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index() => View();

        public IActionResult ExcelExport()
        {
            var products = _repo.GetProducts();
            var dt = new DataTable("Products");
            dt.Columns.AddRange(new[] {
                new DataColumn("ID", typeof(int)),
                new DataColumn("Name", typeof(string)),
                new DataColumn("Price", typeof(double))
            });

            foreach (var p in products)
                dt.Rows.Add(p.ID, p.Name, p.Price);

            using var wb = new XLWorkbook();
            wb.Worksheets.Add(dt);

            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Products.xlsx");
        }

        public IActionResult PdfExport()
        {
            var products = _repo.GetProducts();
            var view = products.Select(p => (p.ID, p.Name, p.Price)).ToList();
            return new Rotativa.AspNetCore.ViewAsPdf("PdfView", view);
        }
    }

}
