using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Entities;

namespace ERPLocadoras.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioResponse?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<UsuarioResponse>> ObterTodosAsync();
        Task<IEnumerable<UsuarioResponse>> ObterPorLocadoraAsync(Guid locadoraId);
        Task<UsuarioResponse> CriarAsync(CriarUsuarioRequest request);
        Task<UsuarioResponse?> AtualizarAsync(Guid id, AtualizarUsuarioRequest request);
        Task<bool> ExcluirAsync(Guid id);
        Task<bool> AlterarStatusAsync(Guid id, bool ativo);
        Task<Usuario?> ObterEntidadePorIdAsync(Guid id);
    }
}