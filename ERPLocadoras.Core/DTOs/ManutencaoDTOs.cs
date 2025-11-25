using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Core.DTOs
{
    public class CriarManutencaoRequest
    {
        public TipoManutencao Tipo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaidaPrevista { get; set; }
        public decimal KmEntrada { get; set; }
        public string? OficinaPrestador { get; set; }
        public string? CnpjContatoOficina { get; set; }
        public Guid? ResponsavelManutencaoId { get; set; }
        public decimal? CustoPecas { get; set; }
        public decimal? CustoMaoDeObra { get; set; }
        public string? GarantiaServico { get; set; }
        public DateTime? DataProximaRevisaoSugerida { get; set; }
        public string? Observacoes { get; set; }
        public Guid VeiculoId { get; set; }
        public Guid LocadoraId { get; set; }
    }

    public class AtualizarManutencaoRequest
    {
        public string? Descricao { get; set; }
        public DateTime? DataSaidaPrevista { get; set; }
        public DateTime? DataSaidaReal { get; set; }
        public decimal? KmSaida { get; set; }
        public string? OficinaPrestador { get; set; }
        public string? CnpjContatoOficina { get; set; }
        public Guid? ResponsavelManutencaoId { get; set; }
        public decimal? CustoPecas { get; set; }
        public decimal? CustoMaoDeObra { get; set; }
        public decimal? CustoTotal { get; set; }
        public string? GarantiaServico { get; set; }
        public DateTime? DataProximaRevisaoSugerida { get; set; }
        public string? Observacoes { get; set; }
        public string? Anexos { get; set; }
        public StatusManutencao? Status { get; set; }
    }

    public class FinalizarManutencaoRequest
    {
        public DateTime DataSaidaReal { get; set; }
        public decimal KmSaida { get; set; }
        public decimal? CustoTotal { get; set; }
        public string? Observacoes { get; set; }
    }

    public class ManutencaoResponse
    {
        public Guid Id { get; set; }
        public TipoManutencao Tipo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaidaPrevista { get; set; }
        public DateTime? DataSaidaReal { get; set; }
        public decimal KmEntrada { get; set; }
        public decimal? KmSaida { get; set; }
        public string? OficinaPrestador { get; set; }
        public string? CnpjContatoOficina { get; set; }
        public Guid? ResponsavelManutencaoId { get; set; }
        public string? ResponsavelManutencaoNome { get; set; }
        public decimal? CustoPecas { get; set; }
        public decimal? CustoMaoDeObra { get; set; }
        public decimal? CustoTotal { get; set; }
        public string? GarantiaServico { get; set; }
        public DateTime? DataProximaRevisaoSugerida { get; set; }
        public string? Observacoes { get; set; }
        public string? Anexos { get; set; }
        public StatusManutencao Status { get; set; }
        public Guid VeiculoId { get; set; }
        public Guid LocadoraId { get; set; }
        public string VeiculoDescricao { get; set; }
        public string LocadoraNome { get; set; }

        // Informações calculadas
        public bool EstaEmAndamento { get; set; }
        public bool EstaConcluida { get; set; }
        public int? DiasManutencao { get; set; }
        public decimal? KmPercorrido { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}