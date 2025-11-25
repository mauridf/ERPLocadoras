namespace ERPLocadoras.Core.Entities
{
    public class Cliente : EntityBase
    {
        public string NomeCompleto { get; private set; }
        public string? NomeSocial { get; private set; }
        public string? Sexo { get; private set; }
        public string? Telefone { get; private set; }
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

        // Foreign Key para Usuario (quando o cliente fizer login)
        public Guid? UsuarioId { get; private set; }

        // Navigation Properties
        public virtual Usuario? Usuario { get; private set; }
        public virtual ICollection<Locacao> Locacoes { get; private set; }

        // Constructor
        public Cliente(string nomeCompleto)
        {
            NomeCompleto = nomeCompleto;
            DataCadastro = DateTime.UtcNow;
            Locacoes = new List<Locacao>();
        }

        // Methods
        public void AtualizarNome(string nomeCompleto)
        {
            NomeCompleto = nomeCompleto;
            AtualizarDataModificacao();
        }

        public void AtualizarDadosPessoais(
            string? nomeSocial,
            string? sexo,
            string? telefone,
            string? fotoUrl)
        {
            NomeSocial = nomeSocial;
            Sexo = sexo;
            Telefone = telefone;
            FotoUrl = fotoUrl;
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

        public void VincularUsuario(Guid usuarioId)
        {
            UsuarioId = usuarioId;
            AtualizarDataModificacao();
        }
    }
}