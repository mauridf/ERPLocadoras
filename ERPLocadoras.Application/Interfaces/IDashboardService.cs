using ERPLocadoras.Core.DTOs;

namespace ERPLocadoras.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardLocadoraResponse> ObterDashboardLocadoraAsync(Guid locadoraId);
        Task<DashboardGlobalResponse> ObterDashboardGlobalAsync();
        Task<IEnumerable<RelatorioLocacoesResponse>> ObterRelatorioLocacoesAsync(Guid locadoraId, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<RelatorioFinanceiroResponse>> ObterRelatorioFinanceiroAsync(Guid locadoraId, DateTime dataInicio, DateTime dataFim);
    }
}