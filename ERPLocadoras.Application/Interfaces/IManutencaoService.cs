using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Entities;
using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Application.Interfaces
{
    public interface IManutencaoService
    {
        Task<ManutencaoResponse?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<ManutencaoResponse>> ObterTodasAsync();
        Task<IEnumerable<ManutencaoResponse>> ObterPorLocadoraAsync(Guid locadoraId);
        Task<IEnumerable<ManutencaoResponse>> ObterPorVeiculoAsync(Guid veiculoId);
        Task<IEnumerable<ManutencaoResponse>> ObterEmAndamentoPorLocadoraAsync(Guid locadoraId);
        Task<ManutencaoResponse> CriarAsync(CriarManutencaoRequest request);
        Task<ManutencaoResponse?> AtualizarAsync(Guid id, AtualizarManutencaoRequest request);
        Task<bool> ExcluirAsync(Guid id);
        Task<bool> FinalizarManutencaoAsync(Guid id, FinalizarManutencaoRequest request);
        Task<bool> AlterarStatusAsync(Guid id, StatusManutencao novoStatus);
        Task<Manutencao?> ObterEntidadePorIdAsync(Guid id);
    }
}