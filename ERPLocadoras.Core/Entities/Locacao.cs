using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Core.Entities
{
    public class Locacao : EntityBase
    {
        // Datas
        public DateTime DataInicio { get; private set; }
        public DateTime DataPrevistaDevolucao { get; private set; }
        public DateTime? DataRealDevolucao { get; private set; }

        // Tipo e Plano
        public TipoLocacao TipoLocacao { get; private set; }
        public string? PlanoLocacao { get; private set; }

        // Valores
        public decimal ValorDiaria { get; private set; }
        public decimal? ValorKmAdicional { get; private set; }
        public decimal? FranquiaKmInclusa { get; private set; }
        public FormaCobranca FormaCobranca { get; private set; }

        // Caução
        public decimal? ValorCaucao { get; private set; }
        public FormaCaucao FormaCaucao { get; private set; }

        // Valores Finais
        public decimal ValorTotalPrevisto { get; private set; }
        public decimal? ValorTotalFinal { get; private set; }
        public decimal? DescontosAcrescimos { get; private set; }

        // Situação
        public SituacaoLocacao Situacao { get; private set; }

        // Responsáveis
        public string? ResponsavelEntrega { get; private set; }
        public string? ResponsavelDevolucao { get; private set; }

        // Checklist
        public string? ChecklistEntrega { get; private set; } // JSON
        public string? ChecklistDevolucao { get; private set; } // JSON

        // Dados do Veículo
        public string? NivelCombustivelEntrega { get; private set; }
        public string? NivelCombustivelDevolucao { get; private set; }
        public decimal KmEntrega { get; private set; }
        public decimal? KmDevolucao { get; private set; }

        // Observações e Anexos
        public string? ObservacoesInternas { get; private set; }
        public string? Anexos { get; private set; } // JSON com URLs

        // Foreign Keys
        public Guid LocadoraId { get; private set; }
        public Guid VeiculoId { get; private set; }
        public Guid ClienteId { get; private set; }

        // Navigation Properties
        public virtual Locadora Locadora { get; private set; }
        public virtual Veiculo Veiculo { get; private set; }
        public virtual Cliente Cliente { get; private set; }

        // Constructor
        public Locacao(
            DateTime dataInicio,
            DateTime dataPrevistaDevolucao,
            TipoLocacao tipoLocacao,
            decimal valorDiaria,
            FormaCobranca formaCobranca,
            FormaCaucao formaCaucao,
            decimal valorTotalPrevisto,
            decimal kmEntrega,
            Guid locadoraId,
            Guid veiculoId,
            Guid clienteId)
        {
            DataInicio = dataInicio;
            DataPrevistaDevolucao = dataPrevistaDevolucao;
            TipoLocacao = tipoLocacao;
            ValorDiaria = valorDiaria;
            FormaCobranca = formaCobranca;
            FormaCaucao = formaCaucao;
            ValorTotalPrevisto = valorTotalPrevisto;
            KmEntrega = kmEntrega;
            LocadoraId = locadoraId;
            VeiculoId = veiculoId;
            ClienteId = clienteId;
            Situacao = SituacaoLocacao.Reservada;
        }

        // Methods
        public void AtualizarPlanoLocacao(string? planoLocacao, decimal? valorKmAdicional, decimal? franquiaKmInclusa)
        {
            PlanoLocacao = planoLocacao;
            ValorKmAdicional = valorKmAdicional;
            FranquiaKmInclusa = franquiaKmInclusa;
            AtualizarDataModificacao();
        }

        public void AtualizarCaucao(decimal? valorCaucao, FormaCaucao formaCaucao)
        {
            ValorCaucao = valorCaucao;
            FormaCaucao = formaCaucao;
            AtualizarDataModificacao();
        }

        public void AtualizarChecklistEntrega(string checklistEntrega, string nivelCombustivel, string responsavelEntrega)
        {
            ChecklistEntrega = checklistEntrega;
            NivelCombustivelEntrega = nivelCombustivel;
            ResponsavelEntrega = responsavelEntrega;
            AtualizarDataModificacao();
        }

        public void AtualizarChecklistDevolucao(string checklistDevolucao, string nivelCombustivel, string responsavelDevolucao)
        {
            ChecklistDevolucao = checklistDevolucao;
            NivelCombustivelDevolucao = nivelCombustivel;
            ResponsavelDevolucao = responsavelDevolucao;
            AtualizarDataModificacao();
        }

        public void IniciarLocacao()
        {
            if (Situacao != SituacaoLocacao.Reservada)
                throw new InvalidOperationException("Só é possível iniciar locações com status Reservada.");

            Situacao = SituacaoLocacao.Ativa;
            AtualizarDataModificacao();
        }

        public void FinalizarLocacao(DateTime dataDevolucao, decimal kmDevolucao, decimal? valorTotalFinal, decimal? descontosAcrescimos)
        {
            if (Situacao != SituacaoLocacao.Ativa && Situacao != SituacaoLocacao.EmAtraso)
                throw new InvalidOperationException("Só é possível finalizar locações ativas ou em atraso.");

            DataRealDevolucao = dataDevolucao;
            KmDevolucao = kmDevolucao;
            ValorTotalFinal = valorTotalFinal;
            DescontosAcrescimos = descontosAcrescimos;
            Situacao = SituacaoLocacao.Finalizada;
            AtualizarDataModificacao();
        }

        public void CancelarLocacao()
        {
            if (Situacao != SituacaoLocacao.Reservada && Situacao != SituacaoLocacao.Ativa)
                throw new InvalidOperationException("Só é possível cancelar locações reservadas ou ativas.");

            Situacao = SituacaoLocacao.Cancelada;
            AtualizarDataModificacao();
        }

        public void MarcarComoAtraso()
        {
            if (Situacao != SituacaoLocacao.Ativa)
                throw new InvalidOperationException("Só é possível marcar como atraso locações ativas.");

            Situacao = SituacaoLocacao.EmAtraso;
            AtualizarDataModificacao();
        }

        public void AtualizarObservacoes(string observacoesInternas, string? anexos)
        {
            ObservacoesInternas = observacoesInternas;
            Anexos = anexos;
            AtualizarDataModificacao();
        }

        // Business Rules
        public bool EstaAtiva()
        {
            return Situacao == SituacaoLocacao.Ativa || Situacao == SituacaoLocacao.EmAtraso;
        }

        public bool EstaEmAtraso()
        {
            return Situacao == SituacaoLocacao.EmAtraso ||
                   (Situacao == SituacaoLocacao.Ativa && DataPrevistaDevolucao < DateTime.UtcNow);
        }

        public int CalcularDiasLocacao()
        {
            var dataFim = DataRealDevolucao ?? DateTime.UtcNow;
            return (int)(dataFim - DataInicio).TotalDays;
        }

        public decimal? CalcularKmRodado()
        {
            return KmDevolucao - KmEntrega;
        }

        public decimal CalcularValorExcedenteKm()
        {
            if (!ValorKmAdicional.HasValue || !FranquiaKmInclusa.HasValue || !KmDevolucao.HasValue)
                return 0;

            var kmRodado = KmDevolucao.Value - KmEntrega;
            var kmExcedente = Math.Max(0, kmRodado - FranquiaKmInclusa.Value);

            return kmExcedente * ValorKmAdicional.Value;
        }

        public void AtualizarDataPrevistaDevolucao(DateTime novaData)
        {
            if (novaData <= DataInicio)
                throw new InvalidOperationException("Data prevista de devolução deve ser após a data de início.");

            DataPrevistaDevolucao = novaData;
            AtualizarDataModificacao();
        }

        public void AtualizarValorDiaria(decimal novoValor)
        {
            ValorDiaria = novoValor;
            AtualizarDataModificacao();
        }

        public void AtualizarValorTotalPrevisto(decimal novoValor)
        {
            ValorTotalPrevisto = novoValor;
            AtualizarDataModificacao();
        }

        public void AtualizarFormaCobranca(FormaCobranca novaForma)
        {
            FormaCobranca = novaForma;
            AtualizarDataModificacao();
        }
    }
}