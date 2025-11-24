namespace ERPLocadoras.Core.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime DataExpiracao { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string TipoUsuario { get; set; }
        public Guid? LocadoraId { get; set; }
        public string LocadoraNome { get; set; }

        public LoginResponse(string token, DateTime dataExpiracao, string email, string nome, string tipoUsuario, Guid? locadoraId, string locadoraNome)
        {
            Token = token;
            DataExpiracao = dataExpiracao;
            Email = email;
            Nome = nome;
            TipoUsuario = tipoUsuario;
            LocadoraId = locadoraId;
            LocadoraNome = locadoraNome;
        }
    }
}