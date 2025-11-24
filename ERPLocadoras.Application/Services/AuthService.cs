using Microsoft.EntityFrameworkCore;
using ERPLocadoras.Core.DTOs;
using ERPLocadoras.Core.Entities;
using ERPLocadoras.Core.Interfaces;
using ERPLocadoras.Application.Interfaces;
using ERPLocadoras.Infra.Data;

namespace ERPLocadoras.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISenhaHasher _senhaHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(
            ApplicationDbContext context,
            ISenhaHasher senhaHasher,
            IJwtTokenService jwtTokenService)
        {
            _context = context;
            _senhaHasher = senhaHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<LoginResponse?> AutenticarAsync(LoginRequest loginRequest)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Locadora)
                .Include(u => u.Pessoa)
                .FirstOrDefaultAsync(u => u.Email == loginRequest.Email && u.Ativo);

            if (usuario == null || !_senhaHasher.VerificarSenha(loginRequest.Senha, usuario.SenhaHash))
                return null;

            if (usuario.EstaExpirado())
                return null;

            var token = _jwtTokenService.GerarToken(usuario);
            var dataExpiracao = _jwtTokenService.GetExpirationDate();

            return new LoginResponse(
                token: token,
                dataExpiracao: dataExpiracao,
                email: usuario.Email,
                nome: usuario.Pessoa?.NomeCompleto ?? usuario.Email,
                tipoUsuario: usuario.Tipo.ToString(),
                locadoraId: usuario.LocadoraId,
                locadoraNome: usuario.Locadora?.NomeFantasia ?? "");
        }

        public async Task<Usuario?> CriarUsuarioAsync(string email, string senha, Core.Enums.UsuarioTipo tipo, Guid? locadoraId = null)
        {
            // Verificar se email já existe
            if (await _context.Usuarios.AnyAsync(u => u.Email == email))
                return null;

            var senhaHash = _senhaHasher.HashSenha(senha);
            var usuario = new Usuario(email, senhaHash, tipo, true, locadoraId);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }
    }
}