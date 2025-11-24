using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Entities;

namespace ERPLocadoras.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> AutenticarAsync(LoginRequest loginRequest);
        Task<Usuario?> CriarUsuarioAsync(string email, string senha, Core.Enums.UsuarioTipo tipo, Guid? locadoraId = null);
    }
}