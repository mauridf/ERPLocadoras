using ERPLocadoras.Core.Interfaces;

namespace ERPLocadoras.Infra.Data.Services
{
    public class SenhaHasher : ISenhaHasher
    {
        public string HashSenha(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public bool VerificarSenha(string senha, string senhaHash)
        {
            return BCrypt.Net.BCrypt.Verify(senha, senhaHash);
        }
    }
}