using ERPLocadoras.Application.Interfaces;

namespace ERPLocadoras.API.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILocadoraService locadoraService)
        {
            // Extrair LocadoraId do token JWT (se existir)
            var locadoraId = context.Items["LocadoraId"] as string;

            if (!string.IsNullOrEmpty(locadoraId) && Guid.TryParse(locadoraId, out var tenantId))
            {
                // Validar se a locadora existe e está ativa
                var locadora = await locadoraService.ObterEntidadePorIdAsync(tenantId);

                if (locadora != null && locadora.Status == Core.Enums.StatusLocadora.Ativa)
                {
                    context.Items["TenantId"] = tenantId;
                    context.Items["Tenant"] = locadora;
                }
            }

            await _next(context);
        }
    }
}