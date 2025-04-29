namespace AppControle.Models.GetDataReform
{
    public class GetDateReform
    {
        public int Id { get; set; }
        public DateTime? Data { get; set; }
        public string? CodObra { get; set; }
        public string? Obra { get; set; }
        public int? Ctr { get; set; }
        public string Pacote { get; set; }
        public string? Empresa { get; set; }
        public int CodItem { get; set; }
        public string? CodService { get; set; }
        public string? DescService { get; set; }
        public double? Qtd { get; set; }
        public string Obs { get; set; }
    }
}
