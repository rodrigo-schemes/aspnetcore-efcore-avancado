namespace EFCoreModulo17.BancoDeDados.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetTenantId(this HttpContext httpContext)
        {
            // Recuperar Tenant pela Rota
            // desenvolvedor.io/tenant-1/product -> " " / "tenant-1" / "product"
            var tenant = httpContext.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries)[0];
            
            // Recuperar Tenant pela Query String
            // desenvolvedor.io/product/?tenantId=tenant-1
            //var tenant = httpContext.Request.QueryString.Value.Split('/', System.StringSplitOptions.RemoveEmptyEntries)[0];
            
            // Recuperar pelo header
            //var tenant = httpContext.Request.Headers["tenant-id"];

            return tenant;
        }
        
    }
}