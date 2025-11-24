using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ERPLocadoras.Core.Models;

namespace ERPLocadoras.API.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtConfig _jwtConfig;

        public JwtMiddleware(RequestDelegate next, IOptions<JwtConfig> jwtConfig)
        {
            _next = next;
            _jwtConfig = jwtConfig.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                AnexarUsuarioAoContexto(context, token);

            await _next(context);
        }

        private void AnexarUsuarioAoContexto(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "nameid").Value;
                var userEmail = jwtToken.Claims.First(x => x.Type == "email").Value;
                var tipoUsuario = jwtToken.Claims.First(x => x.Type == "TipoUsuario").Value;
                var locadoraId = jwtToken.Claims.First(x => x.Type == "LocadoraId").Value;

                // Adicionar claims ao contexto para uso nos controllers
                context.Items["UserId"] = userId;
                context.Items["UserEmail"] = userEmail;
                context.Items["TipoUsuario"] = tipoUsuario;
                context.Items["LocadoraId"] = string.IsNullOrEmpty(locadoraId) ? null : locadoraId;
            }
            catch
            {
                // Token inválido - não fazer nada
            }
        }
    }
}