using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using Rotativa.AspNetCore;
using System.Data;

namespace ExportApp.Controllers
{
    public class ExportController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult ExcelExport()
        {
            var dt = new DataTable("Products");
            dt.Columns.AddRange(new[] {
                new DataColumn("ID", typeof(int)),
                new DataColumn("Name", typeof(string)),
                new DataColumn("Price", typeof(double))
            });

            dt.Rows.Add(1, "Product X", 1000);
            dt.Rows.Add(2, "Product Y", 2000);
            dt.Rows.Add(3, "Product Z", 3000);

            using var wb = new XLWorkbook();
            wb.Worksheets.Add(dt);

            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream.ToArray(), 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                "TestExport.xlsx");
        }

        public IActionResult PdfExport()
        {
            var items = new List<(int ID, string Name, double Price)> {
                (1, "Product X", 1000),
                (2, "Product Y", 2000),
                (3, "Product Z", 3000)
            };

            return new ViewAsPdf("PdfView", items)
            {
                FileName = "TestExport.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }
    }
}
