
using AppControle.Models;
using AppControle.Models.GetData;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Resultset;

[Route("api/exportPDF")]
[ApiController]

public class ExportPDFController : ControllerBase
{
    private readonly AcessControlContext _context;

    public ExportPDFController(AcessControlContext context)
    {
        _context = context;
    }

     async private Task<byte[]> ExportToPDF (List<int> dados)
    {
        using (var memoryStream = new MemoryStream())
        {
            var writer = new PdfWriter(memoryStream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            float[] columnWidths = { 150f, 1000f };

            Table tabela = new Table(columnWidths);
            tabela.AddHeaderCell("Código Serviço");
            tabela.AddHeaderCell("Descrição do Serviço");

            foreach (var dado in dados)
            {
                var item = await _context.GetDateMed.FindAsync(dado);
                tabela.AddCell(item.Serv_itens ?? "N/A").SetFontSize(7);
                tabela.AddCell(item.Descr_Item ?? "N/A").SetFontSize(7);
            }

            document.Add(tabela);
            document.Close();

            return memoryStream.ToArray();
        }
    }

    [HttpPost]
    async public Task<IActionResult> ExportPdf([FromBody] List<int> dados)
    {
        var pdfBytes = await ExportToPDF(dados);
        return File(pdfBytes, "application/pdf", "Medição.pdf");
    }
}