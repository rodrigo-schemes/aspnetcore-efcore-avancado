using EFCoreModulo17.BancoDeDados.Extensions;
using EFCoreModulo17.BancoDeDados.Provider;

namespace EFCoreModulo17.BancoDeDados.Middlewares
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