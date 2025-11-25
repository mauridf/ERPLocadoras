using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Core.DTOs
{
    public class CriarVeiculoRequest
    {
        public TipoVeiculo Tipo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string? VersaoMotorizacao { get; set; }
        public int AnoFabricacao { get; set; }
        public int AnoModelo { get; set; }
        public string Placa { get; set; }
        public string Renavam { get; set; }
        public string Chassi { get; set; }
        public string Cor { get; set; }
        public CategoriaVeiculo Categoria { get; set; }
        public Combustivel Combustivel { get; set; }
        public decimal QuilometragemAtual { get; set; }
        public string? Capacidade { get; set; }
        public DateTime DataAquisicao { get; set; }
        public decimal ValorCompra { get; set; }
        public decimal? ValorMercadoAtual { get; set; }
        public string? ApoliceSeguro { get; set; }
        public string? Seguradora { get; set; }
        public DateTime? VencimentoSeguro { get; set; }
        public string? Documentacao { get; set; }
        public DateTime? DataUltimaRevisao { get; set; }
        public DateTime? DataProximaRevisao { get; set; }
        public string? Observacoes { get; set; }
        public string? FotosAnexos { get; set; }
        public Guid LocadoraId { get; set; }
    }

    public class AtualizarVeiculoRequest
    {
        public string? VersaoMotorizacao { get; set; }
        public string? Capacidade { get; set; }
        public decimal? ValorMercadoAtual { get; set; }
        public string? ApoliceSeguro { get; set; }
        public string? Seguradora { get; set; }
        public DateTime? VencimentoSeguro { get; set; }
        public string? Documentacao { get; set; }
        public DateTime? DataUltimaRevisao { get; set; }
        public DateTime? DataProximaRevisao { get; set; }
        public string? Observacoes { get; set; }
        public string? FotosAnexos { get; set; }
        public StatusVeiculo? Status { get; set; }
    }

    public class AtualizarQuilometragemRequest
    {
        public decimal NovaQuilometragem { get; set; }
    }

    public class VeiculoResponse
    {
        public Guid Id { get; set; }
        public TipoVeiculo Tipo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string? VersaoMotorizacao { get; set; }
        public int AnoFabricacao { get; set; }
        public int AnoModelo { get; set; }
        public string Placa { get; set; }
        public string Renavam { get; set; }
        public string Chassi { get; set; }
        public string Cor { get; set; }
        public CategoriaVeiculo Categoria { get; set; }
        public Combustivel Combustivel { get; set; }
        public decimal QuilometragemAtual { get; set; }
        public string? Capacidade { get; set; }
        public StatusVeiculo Status { get; set; }
        public DateTime DataAquisicao { get; set; }
        public decimal ValorCompra { get; set; }
        public decimal? ValorMercadoAtual { get; set; }
        public string? ApoliceSeguro { get; set; }
        public string? Seguradora { get; set; }
        public DateTime? VencimentoSeguro { get; set; }
        public string? Documentacao { get; set; }
        public DateTime? DataUltimaRevisao { get; set; }
        public DateTime? DataProximaRevisao { get; set; }
        public string? Observacoes { get; set; }
        public string? FotosAnexos { get; set; }
        public Guid LocadoraId { get; set; }
        public string LocadoraNome { get; set; }

        // Informações de disponibilidade
        public bool DisponivelParaLocacao { get; set; }
        public string? MotivoIndisponibilidade { get; set; }

        // Estatísticas
        public int TotalLocacoes { get; set; }
        public int LocacoesAtivas { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}