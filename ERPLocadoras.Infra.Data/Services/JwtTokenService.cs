using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ERPLocadoras.Core.Entities;
using ERPLocadoras.Core.Interfaces;
using ERPLocadoras.Core.Models;

namespace ERPLocadoras.Infra.Data.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtConfig _jwtConfig;

        public JwtTokenService(IOptions<JwtConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig.Value;
        }

        public string GerarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim("TipoUsuario", usuario.Tipo.ToString()),
                new Claim("LocadoraId", usuario.LocadoraId?.ToString() ?? ""),
                new Claim("Ativo", usuario.Ativo.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_jwtConfig.ExpirationInHours),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public DateTime GetExpirationDate()
        {
            return DateTime.UtcNow.AddHours(_jwtConfig.ExpirationInHours);
        }
    }
}