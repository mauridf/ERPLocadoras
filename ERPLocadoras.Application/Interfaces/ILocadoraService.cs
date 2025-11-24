using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Entities;
using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Application.Interfaces
{
    public interface ILocadoraService
    {
        Task<LocadoraResponse?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<LocadoraResponse>> ObterTodasAsync();
        Task<LocadoraResponse> CriarAsync(CriarLocadoraRequest request);
        Task<LocadoraResponse?> AtualizarAsync(Guid id, AtualizarLocadoraRequest request);
        Task<bool> ExcluirAsync(Guid id);
        Task<bool> AlterarStatusAsync(Guid id, StatusLocadora novoStatus);
        Task<Locadora?> ObterEntidadePorIdAsync(Guid id);
    }
}