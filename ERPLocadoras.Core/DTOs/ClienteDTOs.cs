namespace ERPLocadoras.Core.DTOs
{
    public class CriarClienteRequest
    {
        public string NomeCompleto { get; set; }
        public string? NomeSocial { get; set; }
        public string? Sexo { get; set; }
        public string? Telefone { get; set; }
        public string? FotoUrl { get; set; }

        // Dados de Endereço
        public string? CEP { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? UF { get; set; }
        public string? Pais { get; set; }

        // Dados para criação de usuário (opcional)
        public string? Email { get; set; }
        public string? Senha { get; set; }
    }

    public class AtualizarClienteRequest
    {
        public string? NomeCompleto { get; set; }
        public string? NomeSocial { get; set; }
        public string? Sexo { get; set; }
        public string? Telefone { get; set; }
        public string? FotoUrl { get; set; }

        // Dados de Endereço
        public string? CEP { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? UF { get; set; }
        public string? Pais { get; set; }
    }

    public class ClienteResponse
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; }
        public string? NomeSocial { get; set; }
        public string? Sexo { get; set; }
        public string? Telefone { get; set; }
        public DateTime DataCadastro { get; set; }
        public string? FotoUrl { get; set; }

        // Dados de Endereço
        public string? CEP { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? UF { get; set; }
        public string? Pais { get; set; }

        // Dados do usuário (se houver)
        public Guid? UsuarioId { get; set; }
        public string? Email { get; set; }
        public bool TemAcesso { get; set; }

        // Estatísticas
        public int TotalLocacoes { get; set; }
        public int LocacoesAtivas { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }

    public class VincularUsuarioRequest
    {
        public Guid UsuarioId { get; set; }
    }
}