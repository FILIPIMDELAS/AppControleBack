namespace AppControle.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Logradouro { get; set; }
        public string? Bairro { get; set; }
        public string? Numero { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public int? Cep { get; set; }
        public string? Funcao { get; set; }
    }
}
