using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Core.DTOs
{
    public class CriarUsuarioRequest
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public UsuarioTipo Tipo { get; set; }
        public Guid? LocadoraId { get; set; }
        public string? Permissoes { get; set; }
        public DateTime? DataExpiracao { get; set; }

        // Dados da Pessoa (para Admin, Atendente, Mecanico)
        public CriarPessoaRequest? DadosPessoais { get; set; }
    }

    public class AtualizarUsuarioRequest
    {
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public string? Permissoes { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public bool? Ativo { get; set; }

        // Dados da Pessoa
        public AtualizarPessoaRequest? DadosPessoais { get; set; }
    }

    public class UsuarioResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public UsuarioTipo Tipo { get; set; }
        public string? Permissoes { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public bool Ativo { get; set; }
        public Guid? LocadoraId { get; set; }
        public string? LocadoraNome { get; set; }

        // Dados da Pessoa
        public PessoaResponse? DadosPessoais { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }

    public class CriarPessoaRequest
    {
        public string NomeCompleto { get; set; }
        public string? NomeSocial { get; set; }
        public string? Sexo { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
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

    public class AtualizarPessoaRequest
    {
        public string? NomeCompleto { get; set; }
        public string? NomeSocial { get; set; }
        public string? Sexo { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
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

    public class PessoaResponse
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; }
        public string? NomeSocial { get; set; }
        public string? Sexo { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
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

        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}