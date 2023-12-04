using EFCoreModulo17.Tabela.Extensions;
using EFCoreModulo17.Tabela.Provider;

namespace EFCoreModulo17.Tabela.Middlewares
{
    public class TenantMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var tenant = httpContext.RequestServices.GetRequiredService<TenantData>();
            
            tenant.TenantId = httpContext.GetTenantId();

            await next(httpContext);
        }
    }
}