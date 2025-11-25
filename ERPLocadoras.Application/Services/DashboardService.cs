using Microsoft.EntityFrameworkCore;
using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Enums;
using ERPLocadoras.Application.Interfaces;
using ERPLocadoras.Infra.Data;

namespace ERPLocadoras.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardLocadoraResponse> ObterDashboardLocadoraAsync(Guid locadoraId)
        {
            var response = new DashboardLocadoraResponse
            {
                LocacoesPorTipo = new List<DashboardItem>(),
                ReceitaPorMes = new List<DashboardItem>(),
                VeiculosPorCategoria = new List<DashboardItem>(),
                Alertas = new List<AlertaDashboard>()
            };

            // Estatísticas de Veículos
            var veiculos = await _context.Veiculos
                .Where(v => v.LocadoraId == locadoraId)
                .ToListAsync();

            response.TotalVeiculos = veiculos.Count;
            response.VeiculosDisponiveis = veiculos.Count(v => v.Status == StatusVeiculo.Disponivel);
            response.VeiculosLocados = veiculos.Count(v => v.Status == StatusVeiculo.Locado);
            response.VeiculosManutencao = veiculos.Count(v => v.Status == StatusVeiculo.Manutencao);

            // Estatísticas de Locações
            var hoje = DateTime.UtcNow.Date;
            var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);

            var locacoes = await _context.Locacoes
                .Where(l => l.LocadoraId == locadoraId)
                .ToListAsync();

            response.LocacoesAtivas = locacoes.Count(l => l.EstaAtiva());
            response.LocacoesHoje = locacoes.Count(l => l.DataInicio.Date == hoje);
            response.LocacoesAtrasadas = locacoes.Count(l => l.EstaEmAtraso());
            response.LocacoesMes = locacoes.Count(l => l.DataInicio >= inicioMes);

            // Estatísticas Financeiras
            var locacoesMes = locacoes.Where(l => l.DataInicio >= inicioMes && l.ValorTotalFinal.HasValue);
            response.ReceitaMes = locacoesMes.Sum(l => l.ValorTotalFinal ?? 0);

            var locacoesPrevistasMes = locacoes.Where(l => l.DataInicio >= inicioMes);
            response.ReceitaPrevistaMes = locacoesPrevistasMes.Sum(l => l.ValorTotalPrevisto);

            response.ReceitaTotal = locacoes.Where(l => l.ValorTotalFinal.HasValue).Sum(l => l.ValorTotalFinal.Value);

            // Estatísticas de Manutenções
            var manutencoes = await _context.Manutencoes
                .Where(m => m.LocadoraId == locadoraId)
                .ToListAsync();

            response.ManutencoesAndamento = manutencoes.Count(m => m.EstaEmAndamento());
            response.ManutencoesMes = manutencoes.Count(m => m.DataEntrada >= inicioMes);
            response.CustoManutencoesMes = manutencoes
                .Where(m => m.DataEntrada >= inicioMes && m.CustoTotal.HasValue)
                .Sum(m => m.CustoTotal ?? 0);

            // Estatísticas de Clientes
            var clientes = await _context.Clientes.ToListAsync(); // Todos os clientes podem alugar de qualquer locadora
            response.TotalClientes = clientes.Count;
            response.NovosClientesMes = clientes.Count(c => c.DataCadastro >= inicioMes);

            // Dados para Gráficos
            response.LocacoesPorTipo = await ObterLocacoesPorTipo(locadoraId);
            response.ReceitaPorMes = await ObterReceitaPorMes(locadoraId);
            response.VeiculosPorCategoria = await ObterVeiculosPorCategoria(locadoraId);

            // Alertas
            response.Alertas = await ObterAlertasLocadora(locadoraId);

            return response;
        }

        public async Task<DashboardGlobalResponse> ObterDashboardGlobalAsync()
        {
            var response = new DashboardGlobalResponse
            {
                LocadorasPorStatus = new List<DashboardItem>(),
                ReceitaPorLocadora = new List<DashboardItem>()
            };

            // Estatísticas Gerais
            response.TotalLocadoras = await _context.Locadoras.CountAsync();
            response.LocadorasAtivas = await _context.Locadoras.CountAsync(l => l.Status == StatusLocadora.Ativa);
            response.TotalVeiculos = await _context.Veiculos.CountAsync();
            response.TotalClientes = await _context.Clientes.CountAsync();

            var locacoes = await _context.Locacoes.ToListAsync();
            response.LocacoesAtivas = locacoes.Count(l => l.EstaAtiva());
            response.ReceitaTotal = locacoes.Where(l => l.ValorTotalFinal.HasValue).Sum(l => l.ValorTotalFinal.Value);

            // Dados para Gráficos
            response.LocadorasPorStatus = await ObterLocadorasPorStatus();
            response.ReceitaPorLocadora = await ObterReceitaPorLocadora();

            return response;
        }

        public async Task<IEnumerable<RelatorioLocacoesResponse>> ObterRelatorioLocacoesAsync(Guid locadoraId, DateTime dataInicio, DateTime dataFim)
        {
            var locacoes = await _context.Locacoes
                .Where(l => l.LocadoraId == locadoraId && l.DataInicio >= dataInicio && l.DataInicio <= dataFim)
                .ToListAsync();

            var veiculosLocadora = await _context.Veiculos
                .Where(v => v.LocadoraId == locadoraId)
                .CountAsync();

            var resultado = new List<RelatorioLocacoesResponse>();

            for (var data = dataInicio; data <= dataFim; data = data.AddDays(1))
            {
                var locacoesDia = locacoes.Where(l => l.DataInicio.Date == data.Date).ToList();

                resultado.Add(new RelatorioLocacoesResponse
                {
                    Data = data,
                    TotalLocacoes = locacoesDia.Count,
                    LocacoesFinalizadas = locacoesDia.Count(l => l.Situacao == SituacaoLocacao.Finalizada),
                    LocacoesCanceladas = locacoesDia.Count(l => l.Situacao == SituacaoLocacao.Cancelada),
                    Receita = locacoesDia.Where(l => l.ValorTotalFinal.HasValue).Sum(l => l.ValorTotalFinal.Value),
                    TaxaOcupacao = veiculosLocadora > 0 ?
                        (decimal)locacoesDia.Count(l => l.EstaAtiva()) / veiculosLocadora * 100 : 0
                });
            }

            return resultado;
        }

        public async Task<IEnumerable<RelatorioFinanceiroResponse>> ObterRelatorioFinanceiroAsync(Guid locadoraId, DateTime dataInicio, DateTime dataFim)
        {
            var locacoes = await _context.Locacoes
                .Where(l => l.LocadoraId == locadoraId && l.DataInicio >= dataInicio && l.DataInicio <= dataFim)
                .ToListAsync();

            var manutencoes = await _context.Manutencoes
                .Where(m => m.LocadoraId == locadoraId && m.DataEntrada >= dataInicio && m.DataEntrada <= dataFim)
                .ToListAsync();

            var resultado = new List<RelatorioFinanceiroResponse>();

            for (var data = dataInicio; data <= dataFim; data = data.AddDays(1))
            {
                var receitaDia = locacoes
                    .Where(l => l.DataInicio.Date == data.Date && l.ValorTotalFinal.HasValue)
                    .Sum(l => l.ValorTotalFinal.Value);

                var custoManutencoesDia = manutencoes
                    .Where(m => m.DataEntrada.Date == data.Date && m.CustoTotal.HasValue)
                    .Sum(m => m.CustoTotal.Value);

                var lucroLiquido = receitaDia - custoManutencoesDia;
                var taxaLucro = receitaDia > 0 ? (lucroLiquido / receitaDia) * 100 : 0;

                resultado.Add(new RelatorioFinanceiroResponse
                {
                    Data = data,
                    ReceitaLocacoes = receitaDia,
                    CustoManutencoes = custoManutencoesDia,
                    CustoOutros = 0, // Implementar outros custos se necessário
                    LucroLiquido = lucroLiquido,
                    TaxaLucro = taxaLucro
                });
            }

            return resultado;
        }

        private async Task<List<DashboardItem>> ObterLocacoesPorTipo(Guid locadoraId)
        {
            var locacoes = await _context.Locacoes
                .Where(l => l.LocadoraId == locadoraId)
                .GroupBy(l => l.TipoLocacao)
                .Select(g => new DashboardItem
                {
                    Label = g.Key.ToString(),
                    Quantidade = g.Count(),
                    Valor = g.Sum(l => l.ValorTotalFinal ?? 0)
                })
                .ToListAsync();

            return locacoes;
        }

        private async Task<List<DashboardItem>> ObterReceitaPorMes(Guid locadoraId)
        {
            var hoje = DateTime.UtcNow;
            var ultimos6Meses = Enumerable.Range(0, 6)
                .Select(i => hoje.AddMonths(-i))
                .Reverse();

            var resultado = new List<DashboardItem>();

            foreach (var mes in ultimos6Meses)
            {
                var inicioMes = new DateTime(mes.Year, mes.Month, 1);
                var fimMes = inicioMes.AddMonths(1).AddDays(-1);

                var receita = await _context.Locacoes
                    .Where(l => l.LocadoraId == locadoraId &&
                               l.DataInicio >= inicioMes &&
                               l.DataInicio <= fimMes &&
                               l.ValorTotalFinal.HasValue)
                    .SumAsync(l => l.ValorTotalFinal.Value);

                resultado.Add(new DashboardItem
                {
                    Label = mes.ToString("MMM/yyyy"),
                    Valor = receita,
                    Quantidade = 0
                });
            }

            return resultado;
        }

        private async Task<List<DashboardItem>> ObterVeiculosPorCategoria(Guid locadoraId)
        {
            var veiculos = await _context.Veiculos
                .Where(v => v.LocadoraId == locadoraId)
                .GroupBy(v => v.Categoria)
                .Select(g => new DashboardItem
                {
                    Label = g.Key.ToString(),
                    Quantidade = g.Count(),
                    Valor = 0
                })
                .ToListAsync();

            return veiculos;
        }

        private async Task<List<DashboardItem>> ObterLocadorasPorStatus()
        {
            var locadoras = await _context.Locadoras
                .GroupBy(l => l.Status)
                .Select(g => new DashboardItem
                {
                    Label = g.Key.ToString(),
                    Quantidade = g.Count(),
                    Valor = 0
                })
                .ToListAsync();

            return locadoras;
        }

        private async Task<List<DashboardItem>> ObterReceitaPorLocadora()
        {
            var locadoras = await _context.Locadoras.ToListAsync();
            var resultado = new List<DashboardItem>();

            foreach (var locadora in locadoras)
            {
                var receita = await _context.Locacoes
                    .Where(l => l.LocadoraId == locadora.Id && l.ValorTotalFinal.HasValue)
                    .SumAsync(l => l.ValorTotalFinal.Value);

                resultado.Add(new DashboardItem
                {
                    Label = locadora.NomeFantasia,
                    Valor = receita,
                    Quantidade = 0
                });
            }

            return resultado;
        }

        private async Task<List<AlertaDashboard>> ObterAlertasLocadora(Guid locadoraId)
        {
            var alertas = new List<AlertaDashboard>();
            var hoje = DateTime.UtcNow;

            // Alertas de Locações em Atraso
            var locacoesAtraso = await _context.Locacoes
                .Where(l => l.LocadoraId == locadoraId && l.EstaEmAtraso())
                .CountAsync();

            if (locacoesAtraso > 0)
            {
                alertas.Add(new AlertaDashboard
                {
                    Tipo = "LocacoesAtraso",
                    Mensagem = $"{locacoesAtraso} locação(ões) em atraso",
                    Severidade = "warning",
                    Data = hoje
                });
            }

            // Alertas de Manutenções em Andamento
            var manutencoesAndamento = await _context.Manutencoes
                .Where(m => m.LocadoraId == locadoraId && m.EstaEmAndamento())
                .CountAsync();

            if (manutencoesAndamento > 0)
            {
                alertas.Add(new AlertaDashboard
                {
                    Tipo = "ManutencoesAndamento",
                    Mensagem = $"{manutencoesAndamento} manutenção(ões) em andamento",
                    Severidade = "info",
                    Data = hoje
                });
            }

            // Alertas de Seguros Próximos do Vencimento
            var veiculosSeguroVencendo = await _context.Veiculos
                .Where(v => v.LocadoraId == locadoraId &&
                           v.VencimentoSeguro.HasValue &&
                           v.VencimentoSeguro.Value <= hoje.AddDays(15))
                .CountAsync();

            if (veiculosSeguroVencendo > 0)
            {
                alertas.Add(new AlertaDashboard
                {
                    Tipo = "SegurosVencendo",
                    Mensagem = $"{veiculosSeguroVencendo} veículo(s) com seguro próximo do vencimento",
                    Severidade = "warning",
                    Data = hoje
                });
            }

            return alertas;
        }
    }
}