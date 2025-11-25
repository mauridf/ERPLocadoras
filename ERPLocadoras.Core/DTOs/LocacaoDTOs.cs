using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Core.DTOs
{
    public class CriarLocacaoRequest
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataPrevistaDevolucao { get; set; }
        public TipoLocacao TipoLocacao { get; set; }
        public string? PlanoLocacao { get; set; }
        public decimal ValorDiaria { get; set; }
        public decimal? ValorKmAdicional { get; set; }
        public decimal? FranquiaKmInclusa { get; set; }
        public FormaCobranca FormaCobranca { get; set; }
        public decimal? ValorCaucao { get; set; }
        public FormaCaucao FormaCaucao { get; set; }
        public decimal ValorTotalPrevisto { get; set; }
        public decimal KmEntrega { get; set; }
        public Guid LocadoraId { get; set; }
        public Guid VeiculoId { get; set; }
        public Guid ClienteId { get; set; }
        public string? ObservacoesInternas { get; set; }
    }

    public class AtualizarLocacaoRequest
    {
        public DateTime? DataPrevistaDevolucao { get; set; }
        public string? PlanoLocacao { get; set; }
        public decimal? ValorDiaria { get; set; }
        public decimal? ValorKmAdicional { get; set; }
        public decimal? FranquiaKmInclusa { get; set; }
        public FormaCobranca? FormaCobranca { get; set; } // Adicionar esta propriedade
        public decimal? ValorCaucao { get; set; }
        public FormaCaucao? FormaCaucao { get; set; }
        public decimal? ValorTotalPrevisto { get; set; }
        public string? ObservacoesInternas { get; set; }
        public string? Anexos { get; set; }
    }

    public class FinalizarLocacaoRequest
    {
        public DateTime DataRealDevolucao { get; set; }
        public decimal KmDevolucao { get; set; }
        public decimal? ValorTotalFinal { get; set; }
        public decimal? DescontosAcrescimos { get; set; }
        public string ChecklistDevolucao { get; set; }
        public string NivelCombustivelDevolucao { get; set; }
        public string ResponsavelDevolucao { get; set; }
    }

    public class ChecklistEntregaRequest
    {
        public string Checklist { get; set; }
        public string NivelCombustivel { get; set; }
        public string Responsavel { get; set; }
    }

    public class LocacaoResponse
    {
        public Guid Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataPrevistaDevolucao { get; set; }
        public DateTime? DataRealDevolucao { get; set; }
        public TipoLocacao TipoLocacao { get; set; }
        public string? PlanoLocacao { get; set; }
        public decimal ValorDiaria { get; set; }
        public decimal? ValorKmAdicional { get; set; }
        public decimal? FranquiaKmInclusa { get; set; }
        public FormaCobranca FormaCobranca { get; set; }
        public decimal? ValorCaucao { get; set; }
        public FormaCaucao FormaCaucao { get; set; }
        public decimal ValorTotalPrevisto { get; set; }
        public decimal? ValorTotalFinal { get; set; }
        public decimal? DescontosAcrescimos { get; set; }
        public SituacaoLocacao Situacao { get; set; }
        public string? ResponsavelEntrega { get; set; }
        public string? ResponsavelDevolucao { get; set; }
        public string? ChecklistEntrega { get; set; }
        public string? ChecklistDevolucao { get; set; }
        public string? NivelCombustivelEntrega { get; set; }
        public string? NivelCombustivelDevolucao { get; set; }
        public decimal KmEntrega { get; set; }
        public decimal? KmDevolucao { get; set; }
        public string? ObservacoesInternas { get; set; }
        public string? Anexos { get; set; }

        // Foreign Keys
        public Guid LocadoraId { get; set; }
        public Guid VeiculoId { get; set; }
        public Guid ClienteId { get; set; }

        // Navigation Properties (simplificadas)
        public string LocadoraNome { get; set; }
        public string VeiculoDescricao { get; set; }
        public string ClienteNome { get; set; }

        // Informações calculadas
        public bool EstaAtiva { get; set; }
        public bool EstaEmAtraso { get; set; }
        public int DiasLocacao { get; set; }
        public decimal? KmRodado { get; set; }
        public decimal? ValorExcedenteKm { get; set; }
        public decimal? ValorTotalComExcedente { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}