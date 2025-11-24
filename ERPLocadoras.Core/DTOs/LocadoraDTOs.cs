using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Core.DTOs
{
    public class CriarLocadoraRequest
    {
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CNPJ { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public string? CNAEPrincipal { get; set; }
        public string? RegimeTributario { get; set; }
        public DateTime? DataFundacao { get; set; }
        public string? LogotipoUrl { get; set; }

        // Dados de Endereço
        public string? CEP { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? UF { get; set; }
        public string? Pais { get; set; }
        public string? TipoEndereco { get; set; }

        // Dados de Contato
        public string? TelefoneFixo { get; set; }
        public string? CelularWhatsApp { get; set; }
        public string? EmailComercial { get; set; }
        public string? Site { get; set; }
        public string? ResponsavelContato { get; set; }
        public string? CargoResponsavel { get; set; }

        // Configurações Operacionais
        public string? ModalidadeLocacao { get; set; }
        public string? PadraoCobranca { get; set; }
        public string? PoliticaCaucao { get; set; }
        public bool AceitaTerceiros { get; set; }
        public bool IntegracaoSeguradora { get; set; }
        public string? Seguradora { get; set; }
        public string? TipoFrota { get; set; }
    }

    public class AtualizarLocadoraRequest
    {
        public string? RazaoSocial { get; set; }
        public string? NomeFantasia { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public string? CNAEPrincipal { get; set; }
        public string? RegimeTributario { get; set; }
        public DateTime? DataFundacao { get; set; }
        public StatusLocadora? Status { get; set; }
        public string? LogotipoUrl { get; set; }

        // Dados de Endereço
        public string? CEP { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? UF { get; set; }
        public string? Pais { get; set; }
        public string? TipoEndereco { get; set; }

        // Dados de Contato
        public string? TelefoneFixo { get; set; }
        public string? CelularWhatsApp { get; set; }
        public string? EmailComercial { get; set; }
        public string? Site { get; set; }
        public string? ResponsavelContato { get; set; }
        public string? CargoResponsavel { get; set; }
    }

    public class LocadoraResponse
    {
        public Guid Id { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CNPJ { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public string? CNAEPrincipal { get; set; }
        public string? RegimeTributario { get; set; }
        public DateTime? DataFundacao { get; set; }
        public StatusLocadora Status { get; set; }
        public string? LogotipoUrl { get; set; }

        // Dados de Endereço
        public string? CEP { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? UF { get; set; }
        public string? Pais { get; set; }
        public string? TipoEndereco { get; set; }

        // Dados de Contato
        public string? TelefoneFixo { get; set; }
        public string? CelularWhatsApp { get; set; }
        public string? EmailComercial { get; set; }
        public string? Site { get; set; }
        public string? ResponsavelContato { get; set; }
        public string? CargoResponsavel { get; set; }

        // Configurações Operacionais
        public string? ModalidadeLocacao { get; set; }
        public string? PadraoCobranca { get; set; }
        public string? PoliticaCaucao { get; set; }
        public bool AceitaTerceiros { get; set; }
        public bool IntegracaoSeguradora { get; set; }
        public string? Seguradora { get; set; }
        public string? TipoFrota { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        // Estatísticas
        public int TotalUsuarios { get; set; }
        public int TotalVeiculos { get; set; }
        public int TotalLocacoesAtivas { get; set; }
    }
}