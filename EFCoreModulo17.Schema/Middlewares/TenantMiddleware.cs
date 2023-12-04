using EFCoreModulo17.Schema.Extensions;
using EFCoreModulo17.Schema.Provider;

namespace EFCoreModulo17.Schema.Middlewares
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