using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Entities;

namespace ERPLocadoras.Application.Interfaces
{
    public interface IClienteService
    {
        Task<ClienteResponse?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<ClienteResponse>> ObterTodosAsync();
        Task<ClienteResponse> CriarAsync(CriarClienteRequest request);
        Task<ClienteResponse?> AtualizarAsync(Guid id, AtualizarClienteRequest request);
        Task<bool> ExcluirAsync(Guid id);
        Task<bool> VincularUsuarioAsync(Guid clienteId, Guid usuarioId);
        Task<Cliente?> ObterEntidadePorIdAsync(Guid id);
    }
}