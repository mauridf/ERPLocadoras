using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Core.Entities
{
    public class Manutencao : EntityBase
    {
        // Dados da Manutenção
        public TipoManutencao Tipo { get; private set; }
        public string Descricao { get; private set; }
        public DateTime DataEntrada { get; private set; }
        public DateTime? DataSaidaPrevista { get; private set; }
        public DateTime? DataSaidaReal { get; private set; }
        public decimal KmEntrada { get; private set; }
        public decimal? KmSaida { get; private set; }

        // Oficina/Prestador
        public string? OficinaPrestador { get; private set; }
        public string? CnpjContatoOficina { get; private set; }

        // Responsável
        public Guid? ResponsavelManutencaoId { get; private set; }

        // Custos
        public decimal? CustoPecas { get; private set; }
        public decimal? CustoMaoDeObra { get; private set; }
        public decimal? CustoTotal { get; private set; }

        // Garantia e Próxima Revisão
        public string? GarantiaServico { get; private set; } // dias ou KM
        public DateTime? DataProximaRevisaoSugerida { get; private set; }

        // Observações e Anexos
        public string? Observacoes { get; private set; }
        public string? Anexos { get; private set; } // JSON com URLs

        // Status
        public StatusManutencao Status { get; private set; }

        // Foreign Keys
        public Guid VeiculoId { get; private set; }
        public Guid LocadoraId { get; private set; }

        // Navigation Properties
        public virtual Veiculo Veiculo { get; private set; }
        public virtual Locadora Locadora { get; private set; }
        public virtual Usuario? ResponsavelManutencao { get; private set; }

        // Constructor
        public Manutencao(
            TipoManutencao tipo,
            string descricao,
            DateTime dataEntrada,
            decimal kmEntrada,
            Guid veiculoId,
            Guid locadoraId)
        {
            Tipo = tipo;
            Descricao = descricao;
            DataEntrada = dataEntrada;
            KmEntrada = kmEntrada;
            VeiculoId = veiculoId;
            LocadoraId = locadoraId;
            Status = StatusManutencao.Aberta;
        }

        // Methods
        public void AtualizarDatas(DateTime? dataSaidaPrevista, DateTime? dataSaidaReal, decimal? kmSaida)
        {
            DataSaidaPrevista = dataSaidaPrevista;
            DataSaidaReal = dataSaidaReal;
            KmSaida = kmSaida;
            AtualizarDataModificacao();
        }

        public void AtualizarOficina(string? oficinaPrestador, string? cnpjContatoOficina)
        {
            OficinaPrestador = oficinaPrestador;
            CnpjContatoOficina = cnpjContatoOficina;
            AtualizarDataModificacao();
        }

        public void AtualizarResponsavel(Guid? responsavelManutencaoId)
        {
            ResponsavelManutencaoId = responsavelManutencaoId;
            AtualizarDataModificacao();
        }

        public void AtualizarCustos(decimal? custoPecas, decimal? custoMaoDeObra, decimal? custoTotal)
        {
            CustoPecas = custoPecas;
            CustoMaoDeObra = custoMaoDeObra;
            CustoTotal = custoTotal;
            AtualizarDataModificacao();
        }

        public void AtualizarGarantiaProximaRevisao(string? garantiaServico, DateTime? dataProximaRevisaoSugerida)
        {
            GarantiaServico = garantiaServico;
            DataProximaRevisaoSugerida = dataProximaRevisaoSugerida;
            AtualizarDataModificacao();
        }

        public void AtualizarObservacoesAnexos(string? observacoes, string? anexos)
        {
            Observacoes = observacoes;
            Anexos = anexos;
            AtualizarDataModificacao();
        }

        public void AlterarStatus(StatusManutencao novoStatus)
        {
            Status = novoStatus;
            AtualizarDataModificacao();
        }

        public void FinalizarManutencao(DateTime dataSaidaReal, decimal kmSaida, decimal? custoTotal)
        {
            DataSaidaReal = dataSaidaReal;
            KmSaida = kmSaida;
            CustoTotal = custoTotal;
            Status = StatusManutencao.Concluida;
            AtualizarDataModificacao();
        }

        public void AtualizarDescricao(string novaDescricao)
        {
            if (string.IsNullOrWhiteSpace(novaDescricao))
                throw new InvalidOperationException("Descrição não pode ser vazia.");

            Descricao = novaDescricao;
            AtualizarDataModificacao();
        }

        // Business Rules
        public bool EstaEmAndamento()
        {
            return Status == StatusManutencao.EmAndamento;
        }

        public bool EstaConcluida()
        {
            return Status == StatusManutencao.Concluida;
        }

        public int? CalcularDiasManutencao()
        {
            var dataFim = DataSaidaReal ?? DateTime.UtcNow;
            return (int)(dataFim - DataEntrada).TotalDays;
        }

        public decimal? CalcularKmPercorrido()
        {
            return KmSaida - KmEntrada;
        }
    }
}