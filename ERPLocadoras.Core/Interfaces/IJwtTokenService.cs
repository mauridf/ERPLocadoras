using ERPLocadoras.Core.Entities;

namespace ERPLocadoras.Core.Interfaces
{
    public interface IJwtTokenService
    {
        string GerarToken(Usuario usuario);
        DateTime GetExpirationDate();
    }
}