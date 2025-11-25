using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Core.Entities
{
    public class Veiculo : EntityBase
    {
        // Dados Básicos
        public TipoVeiculo Tipo { get; private set; }
        public string Marca { get; private set; }
        public string Modelo { get; private set; }
        public string? VersaoMotorizacao { get; private set; }
        public int AnoFabricacao { get; private set; }
        public int AnoModelo { get; private set; }
        public string Placa { get; private set; }
        public string Renavam { get; private set; }
        public string Chassi { get; private set; }
        public string Cor { get; private set; }
        public CategoriaVeiculo Categoria { get; private set; }
        public Combustivel Combustivel { get; private set; }
        public decimal QuilometragemAtual { get; private set; }
        public string? Capacidade { get; private set; } // m³ ou passageiros
        public StatusVeiculo Status { get; private set; }

        // Dados de Aquisição
        public DateTime DataAquisicao { get; private set; }
        public decimal ValorCompra { get; private set; }
        public decimal? ValorMercadoAtual { get; private set; }

        // Dados de Seguro
        public string? ApoliceSeguro { get; private set; }
        public string? Seguradora { get; private set; }
        public DateTime? VencimentoSeguro { get; private set; }

        // Documentação
        public string? Documentacao { get; private set; } // URLs ou JSON com documentos

        // Manutenção
        public DateTime? DataUltimaRevisao { get; private set; }
        public DateTime? DataProximaRevisao { get; private set; }

        // Observações e Anexos
        public string? Observacoes { get; private set; }
        public string? FotosAnexos { get; private set; } // URLs ou JSON com fotos

        // Foreign Key para Locadora
        public Guid LocadoraId { get; private set; }

        // Navigation Properties
        public virtual Locadora Locadora { get; private set; }
        public virtual ICollection<Locacao> Locacoes { get; private set; }

        // Constructor
        public Veiculo(
            TipoVeiculo tipo,
            string marca,
            string modelo,
            int anoFabricacao,
            int anoModelo,
            string placa,
            string renavam,
            string chassi,
            string cor,
            CategoriaVeiculo categoria,
            Combustivel combustivel,
            decimal quilometragemAtual,
            DateTime dataAquisicao,
            decimal valorCompra,
            Guid locadoraId)
        {
            Tipo = tipo;
            Marca = marca;
            Modelo = modelo;
            AnoFabricacao = anoFabricacao;
            AnoModelo = anoModelo;
            Placa = placa;
            Renavam = renavam;
            Chassi = chassi;
            Cor = cor;
            Categoria = categoria;
            Combustivel = combustivel;
            QuilometragemAtual = quilometragemAtual;
            Status = StatusVeiculo.Disponivel;
            DataAquisicao = dataAquisicao;
            ValorCompra = valorCompra;
            LocadoraId = locadoraId;

            Locacoes = new List<Locacao>();
        }

        // Methods
        public void AtualizarDadosBasicos(
            string? versaoMotorizacao,
            string? capacidade,
            string? observacoes)
        {
            VersaoMotorizacao = versaoMotorizacao;
            Capacidade = capacidade;
            Observacoes = observacoes;
            AtualizarDataModificacao();
        }

        public void AtualizarValorMercado(decimal? valorMercado)
        {
            ValorMercadoAtual = valorMercado;
            AtualizarDataModificacao();
        }

        public void AtualizarDadosSeguro(
            string? apoliceSeguro,
            string? seguradora,
            DateTime? vencimentoSeguro)
        {
            ApoliceSeguro = apoliceSeguro;
            Seguradora = seguradora;
            VencimentoSeguro = vencimentoSeguro;
            AtualizarDataModificacao();
        }

        public void AtualizarDocumentacao(string? documentacao)
        {
            Documentacao = documentacao;
            AtualizarDataModificacao();
        }

        public void AtualizarManutencao(
            DateTime? dataUltimaRevisao,
            DateTime? dataProximaRevisao)
        {
            DataUltimaRevisao = dataUltimaRevisao;
            DataProximaRevisao = dataProximaRevisao;
            AtualizarDataModificacao();
        }

        public void AtualizarFotosAnexos(string? fotosAnexos)
        {
            FotosAnexos = fotosAnexos;
            AtualizarDataModificacao();
        }

        public void AtualizarQuilometragem(decimal novaQuilometragem)
        {
            if (novaQuilometragem < QuilometragemAtual)
                throw new InvalidOperationException("Quilometragem não pode ser menor que a atual.");

            QuilometragemAtual = novaQuilometragem;
            AtualizarDataModificacao();
        }

        public void AlterarStatus(StatusVeiculo novoStatus)
        {
            Status = novoStatus;
            AtualizarDataModificacao();
        }

        public bool EstaDisponivelParaLocacao()
        {
            return Status == StatusVeiculo.Disponivel &&
                   (VencimentoSeguro == null || VencimentoSeguro > DateTime.UtcNow) &&
                   (DataProximaRevisao == null || DataProximaRevisao > DateTime.UtcNow.AddDays(7));
        }
    }
}