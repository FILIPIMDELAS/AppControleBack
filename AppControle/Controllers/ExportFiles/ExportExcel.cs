using AppControle.Models;
using AppControle.Models.GetData;
using iText.Layout;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.IO;

[Route("api/exportExcel")]
[ApiController]

public class ExportEXCELController : ControllerBase
{
    private readonly AcessControlContext _context;

    public ExportEXCELController(AcessControlContext context)
    {
        _context = context;
    }

    async private Task<byte[]> ExportToExcel (List<int> dados)
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Relatório");

            worksheet.Cells[1, 1].Value = "Codigo Serviço";
            worksheet.Cells[1, 2].Value = "Descrição do Serviço";
            int row = 2;

            foreach (var dado in dados)
            {
                var item = await _context.GetDateMed.FindAsync(dado);
                worksheet.Cells[row, 1].Value = item.Serv_itens;
                worksheet.Cells[row, 2].Value = item.Descr_Item;
                row++;
            }

            return package.GetAsByteArray();
        }
    }

    [HttpPost]
    async public Task<IActionResult> ExportExcel(List<int> dados)
    { 
        var excelBytes = await ExportToExcel(dados);
        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Medição.xlsx");
    }

    [HttpGet]
    public async Task<byte[]> ExportReformToExcel(List<DadosReformContract> dados)
    {
        try
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Relatório");
                worksheet.Cells[1, 1].Value = "Empresa";
                worksheet.Cells[1, 2].Value = "Quem";
                worksheet.Cells[1, 3].Value = "Item Serviço";
                worksheet.Cells[1, 4].Value = "Cod. Contrato";
                worksheet.Cells[1, 5].Value = "Cod. Serviço";
                worksheet.Cells[1, 6].Value = "Desc. Serviço";
                worksheet.Cells[1, 7].Value = "Unid.";
                worksheet.Cells[1, 8].Value = "Saldo Contrato";
                worksheet.Cells[1, 9].Value = "Qtd Medida";
                worksheet.Cells[1, 10].Value = "Qtd a reformar";
                int row = 2;

                foreach (var dado in dados)
                {
                    if (dado.qtdRef != 0)
                    {
                        worksheet.Cells[row, 1].Value = dado.skEmpresaObra;
                        worksheet.Cells[row, 2].Value = dado.quem;
                        worksheet.Cells[row, 3].Value = dado.item;
                        worksheet.Cells[row, 4].Value = dado.cod_cont;
                        worksheet.Cells[row, 5].Value = dado.serviço;
                        worksheet.Cells[row, 6].Value = dado.descrição;
                        worksheet.Cells[row, 7].Value = dado.unid;
                        worksheet.Cells[row, 8].Value = dado.saldCont;
                        worksheet.Cells[row, 9].Value = dado.qtdMed;
                        worksheet.Cells[row, 10].Value = dado.qtdRef;
                        row++;
                    }
                }

                byte[] excelBytes = package.GetAsByteArray();
                Console.WriteLine($"Excel gerado com sucesso. Tamanho: {excelBytes.Length} bytes");

                return excelBytes;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao gerar o Excel: {ex.Message}");
            throw;
        }
    }

    [HttpPost("reform")]
    async public Task<IActionResult> ExportExcelReformContract(List<DadosReformContract> dados)
    {
        var excelBytes = await ExportReformToExcel(dados);
        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reforma.xlsx");
    }

    public class DadosReformContract
    {
        public string skEmpresaObra { get; set; }
        public string quem { get; set; }
        public int item { get; set; }
        public int cod_cont { get; set; }
        public string serviço { get; set; }
        public string descrição { get; set; }
        public string unid { get; set; }
        public double saldCont { get; set; }
        public double qtdMed { get; set; }
        public double qtdRef { get; set; }
    }
}