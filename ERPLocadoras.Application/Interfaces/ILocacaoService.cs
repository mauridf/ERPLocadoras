using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Entities;

namespace ERPLocadoras.Application.Interfaces
{
    public interface ILocacaoService
    {
        Task<LocacaoResponse?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<LocacaoResponse>> ObterTodasAsync();
        Task<IEnumerable<LocacaoResponse>> ObterPorLocadoraAsync(Guid locadoraId);
        Task<IEnumerable<LocacaoResponse>> ObterPorClienteAsync(Guid clienteId);
        Task<IEnumerable<LocacaoResponse>> ObterAtivasPorLocadoraAsync(Guid locadoraId);
        Task<LocacaoResponse> CriarAsync(CriarLocacaoRequest request);
        Task<LocacaoResponse?> AtualizarAsync(Guid id, AtualizarLocacaoRequest request);
        Task<bool> ExcluirAsync(Guid id);
        Task<bool> IniciarLocacaoAsync(Guid id, ChecklistEntregaRequest checklist);
        Task<bool> FinalizarLocacaoAsync(Guid id, FinalizarLocacaoRequest request);
        Task<bool> CancelarLocacaoAsync(Guid id);
        Task<bool> MarcarComoAtrasoAsync(Guid id);
        Task<Locacao?> ObterEntidadePorIdAsync(Guid id);
    }
}