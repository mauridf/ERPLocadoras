using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Core.Entities
{
    public class Pessoa : EntityBase
    {
        public string NomeCompleto { get; private set; }
        public string? NomeSocial { get; private set; }
        public string? Sexo { get; private set; }
        public string? Telefone { get; private set; }
        public DateTime? DataNascimento { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public string? FotoUrl { get; private set; }

        // Dados de Endereço
        public string? CEP { get; private set; }
        public string? Logradouro { get; private set; }
        public string? Numero { get; private set; }
        public string? Complemento { get; private set; }
        public string? Bairro { get; private set; }
        public string? Cidade { get; private set; }
        public string? UF { get; private set; }
        public string? Pais { get; private set; }

        // Foreign Key para Usuario
        public Guid UsuarioId { get; private set; }

        // Navigation Properties
        public virtual Usuario Usuario { get; private set; }

        // Constructor
        public Pessoa(string nomeCompleto, Guid usuarioId)
        {
            NomeCompleto = nomeCompleto;
            UsuarioId = usuarioId;
            DataCadastro = DateTime.UtcNow;
        }

        // Methods
        public void AtualizarDadosPessoais(
            string? nomeSocial,
            string? sexo,
            string? telefone,
            DateTime? dataNascimento,
            string? fotoUrl)
        {
            NomeSocial = nomeSocial;
            Sexo = sexo;
            Telefone = telefone;
            DataNascimento = dataNascimento;
            FotoUrl = fotoUrl;
            AtualizarDataModificacao();
        }

        public void AtualizarNomeCompleto(string nomeCompleto)
        {
            if (!string.IsNullOrWhiteSpace(nomeCompleto))
                NomeCompleto = nomeCompleto;

            AtualizarDataModificacao();
        }

        public void AtualizarEndereco(
            string? cep,
            string? logradouro,
            string? numero,
            string? complemento,
            string? bairro,
            string? cidade,
            string? uf,
            string? pais)
        {
            CEP = cep;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            UF = uf;
            Pais = pais;
            AtualizarDataModificacao();
        }
    }
}