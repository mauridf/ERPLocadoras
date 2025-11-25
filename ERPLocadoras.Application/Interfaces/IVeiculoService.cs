using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Entities;
using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Application.Interfaces
{
    public interface IVeiculoService
    {
        Task<VeiculoResponse?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<VeiculoResponse>> ObterTodosAsync();
        Task<IEnumerable<VeiculoResponse>> ObterPorLocadoraAsync(Guid locadoraId);
        Task<IEnumerable<VeiculoResponse>> ObterDisponiveisPorLocadoraAsync(Guid locadoraId);
        Task<VeiculoResponse> CriarAsync(CriarVeiculoRequest request);
        Task<VeiculoResponse?> AtualizarAsync(Guid id, AtualizarVeiculoRequest request);
        Task<bool> ExcluirAsync(Guid id);
        Task<bool> AtualizarQuilometragemAsync(Guid id, decimal novaQuilometragem);
        Task<bool> AlterarStatusAsync(Guid id, StatusVeiculo novoStatus);
        Task<Veiculo?> ObterEntidadePorIdAsync(Guid id);
    }
}