using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Core.Entities
{
    public class Locadora : EntityBase
    {
        // Dados Gerais
        public string RazaoSocial { get; private set; }
        public string NomeFantasia { get; private set; }
        public string CNPJ { get; private set; }
        public string? InscricaoEstadual { get; private set; }
        public string? InscricaoMunicipal { get; private set; }
        public string? CNAEPrincipal { get; private set; }
        public string? RegimeTributario { get; private set; }
        public DateTime? DataFundacao { get; private set; }
        public StatusLocadora Status { get; private set; }
        public string? LogotipoUrl { get; private set; }

        // Dados de Endereço
        public string? CEP { get; private set; }
        public string? Logradouro { get; private set; }
        public string? Numero { get; private set; }
        public string? Complemento { get; private set; }
        public string? Bairro { get; private set; }
        public string? Cidade { get; private set; }
        public string? UF { get; private set; }
        public string? Pais { get; private set; }
        public string? TipoEndereco { get; private set; }

        // Dados de Contato
        public string? TelefoneFixo { get; private set; }
        public string? CelularWhatsApp { get; private set; }
        public string? EmailComercial { get; private set; }
        public string? Site { get; private set; }
        public string? ResponsavelContato { get; private set; }
        public string? CargoResponsavel { get; private set; }

        // Configurações Operacionais
        public string? ModalidadeLocacao { get; private set; }
        public string? PadraoCobranca { get; private set; }
        public string? PoliticaCaucao { get; private set; }
        public bool AceitaTerceiros { get; private set; }
        public bool IntegracaoSeguradora { get; private set; }
        public string? Seguradora { get; private set; }
        public string? TipoFrota { get; private set; }

        // Navigation Properties
        public virtual ICollection<Usuario> Usuarios { get; private set; }
        public virtual ICollection<Veiculo> Veiculos { get; private set; }
        public virtual ICollection<Locacao> Locacoes { get; private set; }

        // Constructor
        public Locadora(
            string razaoSocial,
            string nomeFantasia,
            string cnpj,
            StatusLocadora status)
        {
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            CNPJ = cnpj;
            Status = status;

            Usuarios = new List<Usuario>();
            Veiculos = new List<Veiculo>();
            Locacoes = new List<Locacao>();
        }

        private Locadora()
        {
            Usuarios = new List<Usuario>();
            Veiculos = new List<Veiculo>();
            Locacoes = new List<Locacao>();
        }


        // ==== MÉTODO NOVO ====
        public void AtualizarDadosBasicos(string? razaoSocial, string? nomeFantasia)
        {
            if (!string.IsNullOrWhiteSpace(razaoSocial))
                RazaoSocial = razaoSocial;

            if (!string.IsNullOrWhiteSpace(nomeFantasia))
                NomeFantasia = nomeFantasia;

            AtualizarDataModificacao();
        }


        public void AtualizarDadosGerais(
            string? inscricaoEstadual,
            string? inscricaoMunicipal,
            string? cnaePrincipal,
            string? regimeTributario,
            DateTime? dataFundacao,
            string? logotipoUrl)
        {
            InscricaoEstadual = inscricaoEstadual;
            InscricaoMunicipal = inscricaoMunicipal;
            CNAEPrincipal = cnaePrincipal;
            RegimeTributario = regimeTributario;
            DataFundacao = dataFundacao;
            LogotipoUrl = logotipoUrl;
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
            string? pais,
            string? tipoEndereco)
        {
            CEP = cep;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            UF = uf;
            Pais = pais;
            TipoEndereco = tipoEndereco;
            AtualizarDataModificacao();
        }

        public void AtualizarContato(
            string? telefoneFixo,
            string? celularWhatsApp,
            string? emailComercial,
            string? site,
            string? responsavelContato,
            string? cargoResponsavel)
        {
            TelefoneFixo = telefoneFixo;
            CelularWhatsApp = celularWhatsApp;
            EmailComercial = emailComercial;
            Site = site;
            ResponsavelContato = responsavelContato;
            CargoResponsavel = cargoResponsavel;
            AtualizarDataModificacao();
        }

        public void AtualizarConfiguracoesOperacionais(
            string? modalidadeLocacao,
            string? padraoCobranca,
            string? politicaCaucao,
            bool aceitaTerceiros,
            bool integracaoSeguradora,
            string? seguradora,
            string? tipoFrota)
        {
            ModalidadeLocacao = modalidadeLocacao;
            PadraoCobranca = padraoCobranca;
            PoliticaCaucao = politicaCaucao;
            AceitaTerceiros = aceitaTerceiros;
            IntegracaoSeguradora = integracaoSeguradora;
            Seguradora = seguradora;
            TipoFrota = tipoFrota;
            AtualizarDataModificacao();
        }

        public void AlterarStatus(StatusLocadora novoStatus)
        {
            Status = novoStatus;
            AtualizarDataModificacao();
        }
    }
}
