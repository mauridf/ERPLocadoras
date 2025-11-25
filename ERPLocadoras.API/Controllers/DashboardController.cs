using Microsoft.AspNetCore.Mvc;
using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Application.Interfaces;

namespace ERPLocadoras.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("locadora/{locadoraId}")]
        public async Task<ActionResult<DashboardLocadoraResponse>> ObterDashboardLocadora(Guid locadoraId)
        {
            var dashboard = await _dashboardService.ObterDashboardLocadoraAsync(locadoraId);
            return Ok(dashboard);
        }

        [HttpGet("global")]
        public async Task<ActionResult<DashboardGlobalResponse>> ObterDashboardGlobal()
        {
            var dashboard = await _dashboardService.ObterDashboardGlobalAsync();
            return Ok(dashboard);
        }

        [HttpGet("locadora/{locadoraId}/relatorio-locacoes")]
        public async Task<ActionResult<IEnumerable<RelatorioLocacoesResponse>>> ObterRelatorioLocacoes(
            Guid locadoraId,
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim)
        {
            var relatorio = await _dashboardService.ObterRelatorioLocacoesAsync(locadoraId, dataInicio, dataFim);
            return Ok(relatorio);
        }

        [HttpGet("locadora/{locadoraId}/relatorio-financeiro")]
        public async Task<ActionResult<IEnumerable<RelatorioFinanceiroResponse>>> ObterRelatorioFinanceiro(
            Guid locadoraId,
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim)
        {
            var relatorio = await _dashboardService.ObterRelatorioFinanceiroAsync(locadoraId, dataInicio, dataFim);
            return Ok(relatorio);
        }
    }
}