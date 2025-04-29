using AppControle.Models.GetData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Net.Http.Headers;
using Microsoft.Identity.Client;
using AppControle.Models;


[Route("api/getDate")]
[ApiController]
public class GetDateController : ControllerBase
{
    private readonly AcessControlContext _context;

    public GetDateController(AcessControlContext context)
    {
        _context = context;
    }

    [HttpPost("Med")]
    public async Task<ActionResult> PostMedicao(IFormFile file)
    {
        if (file == null)
        {
            return BadRequest("Arquivo não encontrado!");
        }

        using (var strem = new MemoryStream())
        {
            await file.CopyToAsync(strem);
            using (var packege = new ExcelPackage(strem))
            {
                var worksheet = packege.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int i = 2; i <= rowCount; i++)
                {
                    var empObraCont = worksheet.Cells[i, 1].Text;
                    var empObraContServ = worksheet.Cells[i, 2].Text;
                    var empObraItem = worksheet.Cells[i, 3].Text;
                    var empresa_cont = int.TryParse(worksheet.Cells[i, 4].Text, out var mpresa_cont) ? mpresa_cont : (int?)null;
                    var obra_cont = worksheet.Cells[i, 5].Text;
                    var cod_cont = int.TryParse(worksheet.Cells[i, 6].Text, out var od_cont) ? od_cont : (int?)null;
                    var serv_itens = worksheet.Cells[i, 7].Text;
                    var qtdeTotalMedida = double.TryParse(worksheet.Cells[i, 8].Text, out var tdeTotalMedida) ? tdeTotalMedida : (double?)null;
                    var totalMedicao = double.TryParse(worksheet.Cells[i, 9].Text, out var otalMedicao) ? otalMedicao : (double?)null;
                    var saldoContrato = double.TryParse(worksheet.Cells[i, 10].Text, out var aldoContrato) ? aldoContrato : (double?)null;
                    var item_itens = int.TryParse(worksheet.Cells[i, 11].Text, out var tem_itens) ? tem_itens : (int?)null;
                    var cod_Item = worksheet.Cells[i, 12].Text;
                    var descr_Item = worksheet.Cells[i, 13].Text;

                    var produtoExistente = _context.GetDateMed.FirstOrDefault(p => p.EmpObraCont == empObraCont || p.EmpObraContServ == empObraContServ || p.EmpObraItem == empObraItem ||
                        p.Empresa_cont == empresa_cont || p.Obra_cont == obra_cont || p.Cod_cont == cod_cont || p.Serv_itens == serv_itens || p.QtdeTotalMedida == qtdeTotalMedida || p.TotalMedicao == totalMedicao ||
                        p.SaldoContrato == saldoContrato || p.Item_itens == item_itens || p.Cod_Item == cod_Item || p.Descr_Item == descr_Item);
                    if (produtoExistente == null)
                    {
                        var getDateMed = new GetDateMed
                        {
                            EmpObraCont = empObraCont,
                            EmpObraContServ = empObraContServ,
                            EmpObraItem = empObraItem,
                            Empresa_cont = empresa_cont,
                            Obra_cont = obra_cont,
                            Cod_cont = cod_cont,
                            Serv_itens = serv_itens,
                            QtdeTotalMedida = qtdeTotalMedida,
                            TotalMedicao = totalMedicao,
                            SaldoContrato = saldoContrato,
                            Item_itens = item_itens,
                            Cod_Item = cod_Item,
                            Descr_Item = descr_Item,
                        };

                        _context.GetDateMed.Add(getDateMed);
                    }
                    
                }

                await _context.SaveChangesAsync();
            }
        }

        return Ok("Dados importados com sucesso.");
    }

    [HttpPost("Orc")]
    public async Task<ActionResult> PostOrcamento(IFormFile file)
    {
        if (file == null)
        {
            return BadRequest("Arquivo não encontrado!");
        }

        using (var strem = new MemoryStream())
        {
            await file.CopyToAsync(strem);
            using (var packege = new ExcelPackage(strem))
            {
                var worksheet = packege.Workbook.Worksheets[2];
                var rowCount = worksheet.Dimension.Rows;

                for (int i = 2; i <= rowCount; i++)
                {
                    var empObraServico = worksheet.Cells[i, 1].Text;
                    var empresa_pla = int.TryParse(worksheet.Cells[i, 2].Text, out var mpresa_pla) ? mpresa_pla : (int?)null;
                    var obra_pla = worksheet.Cells[i, 3].Text;
                    var serv_pla = worksheet.Cells[i, 4].Text;
                    var qdade_pla = worksheet.Cells[i, 5].Text;
                    var precoUnit = double.TryParse(worksheet.Cells[i, 6].Text, out var recoUnit) ? recoUnit : (double?)null;
                    var custoTotal = double.TryParse(worksheet.Cells[i, 7].Text, out var ustoTotal) ? ustoTotal : (double?)null;
                    var desc_orc = worksheet.Cells[i, 8].Text;

                    var produtoExistente = _context.GetDateOrc.FirstOrDefault(p => p.EmpObraServico == empObraServico || p.Empresa_pla == empresa_pla || p.Obra_pla == obra_pla || p.Serv_pla == serv_pla ||
                    p.Qdade_pla == qdade_pla || p.PrecoUnit == precoUnit || p.CustoTotal == custoTotal || p.Desc_orc == desc_orc);
                    if (produtoExistente == null)
                    {
                        var getDateOrc = new GetDateOrc
                        {
                            EmpObraServico = empObraServico,
                            Empresa_pla = empresa_pla,
                            Obra_pla = obra_pla,
                            Serv_pla = serv_pla,
                            Qdade_pla = qdade_pla,
                            PrecoUnit = precoUnit,
                            CustoTotal = custoTotal,
                            Desc_orc = desc_orc,
                        };

                        _context.GetDateOrc.Add(getDateOrc);
                    }
                    
                }

                await _context.SaveChangesAsync();
            }
        }

        return Ok("Dados importados com sucesso.");
    }

    [HttpPost("Cont")]
    public async Task<ActionResult> PostContrato(IFormFile file)
    {
        if (file == null)
        {
            return BadRequest("Arquivo não encontrado!");
        }

        using (var strem = new MemoryStream())
        {
            await file.CopyToAsync(strem);
            using (var packege = new ExcelPackage(strem))
            {
                var worksheet = packege.Workbook.Worksheets[1];
                var rowCount = worksheet.Dimension.Rows;

                for (int i = 2; i <= rowCount; i++)
                {
                    var skEmpresaObra = worksheet.Cells[i, 1].Text;
                    var skEmpresaObraServico = worksheet.Cells[i, 2].Text;
                    var skEmpresaObraContServico = worksheet.Cells[i, 3].Text;
                    var skEmpresaObraContrato = worksheet.Cells[i, 4].Text;
                    var empContrato = worksheet.Cells[i, 5].Text;
                    var empresa_cont = int.TryParse(worksheet.Cells[i, 6].Text, out var mpresa_cont) ? mpresa_cont : (int?)null;
                    var obra_cont = worksheet.Cells[i, 7].Text;
                    var cod_cont = int.TryParse(worksheet.Cells[i, 8].Text, out var od_cont) ? od_cont : (int?)null;
                    var dtCriaçãoContrato = DateTime.TryParse(worksheet.Cells[i, 9].Text, out var tCriaçãoContrato) ? tCriaçãoContrato : (DateTime?)null;
                    var contratada = worksheet.Cells[i, 10].Text;
                    var item = int.TryParse(worksheet.Cells[i, 11].Text, out var tem) ? tem : (int?)null;
                    var serviço = worksheet.Cells[i, 12].Text;
                    var descrição = worksheet.Cells[i, 13].Text;
                    var unid = worksheet.Cells[i, 14].Text;
                    var qtde = double.TryParse(worksheet.Cells[i, 15].Text, out var tde) ? tde : (double?)null;
                    var preço = double.TryParse(worksheet.Cells[i, 16].Text, out var reço) ? reço : (double?)null;
                    var total = double.TryParse(worksheet.Cells[i, 17].Text, out var otal) ? otal : (double?)null;
                    var quem = worksheet.Cells[i, 18].Text;
                    var situacao = worksheet.Cells[i, 19].Text;
                    var status = worksheet.Cells[i, 20].Text;

                    var produtoExistente = _context.GetDateCont.FirstOrDefault(p => p.SkEmpresaObra == skEmpresaObra || p.SkEmpresaObraServico == skEmpresaObraServico || p.SkEmpresaObraContServico == skEmpresaObraContServico ||
                    p.SkEmpresaObraContrato == skEmpresaObraContrato || p.EmpContrato == empContrato || p.Empresa_cont == empresa_cont || p.Obra_cont == obra_cont || p.Cod_cont == cod_cont ||
                    p.DtCriaçãoContrato == dtCriaçãoContrato || p.Contratada == contratada || p.Item == item || p.Serviço == serviço || p.Descrição == descrição ||
                    p.Unid == unid || p.Qtde == qtde || p.Preço == preço || p.Total == total || p.Quem == quem || p.Situacao == situacao || p.Status == status);
                    if (produtoExistente == null)
                    {
                        var getDateCont = new GetDateCont
                        {
                            SkEmpresaObra = skEmpresaObra,
                            SkEmpresaObraServico = skEmpresaObraServico,
                            SkEmpresaObraContServico = skEmpresaObraContServico,
                            SkEmpresaObraContrato = skEmpresaObraContrato,
                            EmpContrato = empContrato,
                            Empresa_cont = empresa_cont,
                            Obra_cont = obra_cont,
                            Cod_cont = cod_cont,
                            DtCriaçãoContrato = dtCriaçãoContrato,
                            Contratada = contratada,
                            Item = item,
                            Serviço = serviço,
                            Descrição = descrição,
                            Unid = unid,
                            Qtde = qtde,
                            Preço = preço,
                            Total = total,
                            Quem = quem,
                            Situacao = situacao,
                            Status = status,
                        };

                        _context.GetDateCont.Add(getDateCont);
                    }

                }

                await _context.SaveChangesAsync();
            }
        }

        return Ok("Dados importados com sucesso.");
    }

    [HttpGet("Cont")]
    public async Task<ActionResult<IEnumerable<GetDateCont>>> GetAllDateCont()
    {
        return await _context.GetDateCont.Select(x => ItemDTOCont(x)).ToListAsync();
    }

    [HttpGet("Med")]
    public async Task<ActionResult<IEnumerable<GetDateMed>>> GetAllDateMed()
    {
        return await _context.GetDateMed.Select(x => ItemDTOMed(x)).ToListAsync();
    }

    [HttpGet("Orc")]
    public async Task<ActionResult<IEnumerable<GetDateOrc>>> GetAllDateOrc()
    {
        return await _context.GetDateOrc.Select(x => ItemDTOOrc(x)).ToListAsync();
    }

    [HttpGet("Cont/{id}")]
    public async Task<ActionResult<GetDateCont>> GetContratoById(int id)
    {
        var Usuario = await _context.GetDateCont.FindAsync(id);

        if (Usuario == null)
        {
            return NotFound();
        }

        return ItemDTOCont(Usuario);
    }

    [HttpGet("Med/{id}")]
    public async Task<ActionResult<GetDateMed>> GetMedicaoById(int id)
    {
        var Usuario = await _context.GetDateMed.FindAsync(id);

        if (Usuario == null)
        {
            return NotFound();
        }

        return ItemDTOMed(Usuario);
    }

    [HttpGet("Orc/{id}")]
    public async Task<ActionResult<GetDateOrc>> GetOrcamentoById(int id)
    {
        var Usuario = await _context.GetDateOrc.FindAsync(id);

        if (Usuario == null)
        {
            return NotFound();
        }

        return ItemDTOOrc(Usuario);
    }

    private static GetDateCont ItemDTOCont(GetDateCont getDateCont) =>
    new GetDateCont
    {
        Id = getDateCont.Id,
        SkEmpresaObra = getDateCont.SkEmpresaObra,
        SkEmpresaObraServico = getDateCont.SkEmpresaObraServico,
        SkEmpresaObraContServico = getDateCont.SkEmpresaObraContServico,
        SkEmpresaObraContrato = getDateCont.SkEmpresaObraContrato,
        EmpContrato = getDateCont.EmpContrato,
        Empresa_cont = getDateCont.Empresa_cont,
        Obra_cont = getDateCont.Obra_cont,
        Cod_cont = getDateCont.Cod_cont,
        DtCriaçãoContrato = getDateCont.DtCriaçãoContrato,
        Contratada = getDateCont.Contratada,
        Item = getDateCont.Item,
        Serviço = getDateCont.Serviço,
        Descrição = getDateCont.Descrição,
        Unid = getDateCont.Unid,
        Qtde = getDateCont.Qtde,
        Preço = getDateCont.Preço,
        Total = getDateCont.Total,
        Quem = getDateCont.Quem,
        Situacao = getDateCont.Situacao,
        Status = getDateCont.Status,
    };

    private static GetDateMed ItemDTOMed(GetDateMed getDateMed) =>
    new GetDateMed
    {
        Id = getDateMed.Id,
        EmpObraCont = getDateMed.EmpObraCont,
        EmpObraContServ = getDateMed.EmpObraContServ,
        EmpObraItem = getDateMed.EmpObraItem,
        Empresa_cont = getDateMed.Empresa_cont,
        Obra_cont = getDateMed.Obra_cont,
        Cod_cont = getDateMed.Cod_cont,
        Serv_itens = getDateMed.Serv_itens,
        QtdeTotalMedida = getDateMed.QtdeTotalMedida,
        TotalMedicao = getDateMed.TotalMedicao,
        SaldoContrato = getDateMed.SaldoContrato,
        Item_itens = getDateMed.Item_itens,
        Cod_Item = getDateMed.Cod_Item,
        Descr_Item = getDateMed.Descr_Item,
    };

    private static GetDateOrc ItemDTOOrc(GetDateOrc getDateOrc) =>
    new GetDateOrc
    {
        Id = getDateOrc.Id,
        EmpObraServico = getDateOrc.EmpObraServico,
        Empresa_pla = getDateOrc.Empresa_pla,
        Obra_pla = getDateOrc.Obra_pla,
        Serv_pla = getDateOrc.Serv_pla,
        Qdade_pla = getDateOrc.Qdade_pla,
        PrecoUnit = getDateOrc.PrecoUnit,
        CustoTotal = getDateOrc.CustoTotal,
        Desc_orc = getDateOrc.Desc_orc,
    };
}