namespace AppControle.Models.GetData
{
    public class GetDateMed
    {
        public int Id { get; set; }
        public string? EmpObraCont { get; set; }
        public string? EmpObraContServ { get; set; }
        public string? EmpObraItem { get; set; }
        public int? Empresa_cont { get; set; }
        public string? Obra_cont { get; set; }
        public int? Cod_cont { get; set; }
        public string? Serv_itens { get; set; }
        public double? QtdeTotalMedida { get; set; }
        public double? TotalMedicao { get; set; }
        public double? SaldoContrato { get; set; }
        public int? Item_itens { get; set; }
        public string? Cod_Item { get; set; }
        public string? Descr_Item { get; set; }
    }
}
