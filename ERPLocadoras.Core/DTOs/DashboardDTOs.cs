namespace ERPLocadoras.Core.DTOs
{
    public class DashboardLocadoraResponse
    {
        // Estatísticas Gerais
        public int TotalVeiculos { get; set; }
        public int VeiculosDisponiveis { get; set; }
        public int VeiculosLocados { get; set; }
        public int VeiculosManutencao { get; set; }

        // Locações
        public int LocacoesAtivas { get; set; }
        public int LocacoesHoje { get; set; }
        public int LocacoesAtrasadas { get; set; }
        public int LocacoesMes { get; set; }

        // Financeiro
        public decimal ReceitaMes { get; set; }
        public decimal ReceitaPrevistaMes { get; set; }
        public decimal ReceitaTotal { get; set; }

        // Manutenções
        public int ManutencoesAndamento { get; set; }
        public int ManutencoesMes { get; set; }
        public decimal CustoManutencoesMes { get; set; }

        // Clientes
        public int TotalClientes { get; set; }
        public int NovosClientesMes { get; set; }

        // Gráficos
        public List<DashboardItem> LocacoesPorTipo { get; set; }
        public List<DashboardItem> ReceitaPorMes { get; set; }
        public List<DashboardItem> VeiculosPorCategoria { get; set; }

        // Alertas
        public List<AlertaDashboard> Alertas { get; set; }
    }

    public class DashboardGlobalResponse
    {
        public int TotalLocadoras { get; set; }
        public int LocadorasAtivas { get; set; }
        public int TotalVeiculos { get; set; }
        public int TotalClientes { get; set; }
        public int LocacoesAtivas { get; set; }
        public decimal ReceitaTotal { get; set; }
        public List<DashboardItem> LocadorasPorStatus { get; set; }
        public List<DashboardItem> ReceitaPorLocadora { get; set; }
    }

    public class RelatorioLocacoesResponse
    {
        public DateTime Data { get; set; }
        public int TotalLocacoes { get; set; }
        public int LocacoesFinalizadas { get; set; }
        public int LocacoesCanceladas { get; set; }
        public decimal Receita { get; set; }
        public decimal TaxaOcupacao { get; set; }
    }

    public class RelatorioFinanceiroResponse
    {
        public DateTime Data { get; set; }
        public decimal ReceitaLocacoes { get; set; }
        public decimal CustoManutencoes { get; set; }
        public decimal CustoOutros { get; set; }
        public decimal LucroLiquido { get; set; }
        public decimal TaxaLucro { get; set; }
    }

    public class DashboardItem
    {
        public string Label { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
    }

    public class AlertaDashboard
    {
        public string Tipo { get; set; }
        public string Mensagem { get; set; }
        public string Severidade { get; set; } // info, warning, error
        public DateTime Data { get; set; }
    }
}