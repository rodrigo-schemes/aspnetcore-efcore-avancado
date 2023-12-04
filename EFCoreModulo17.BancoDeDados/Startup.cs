using EFCoreModulo17.BancoDeDados.Data;
using EFCoreModulo17.BancoDeDados.Extensions;
using EFCoreModulo17.BancoDeDados.Provider;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace EFCoreModulo17.BancoDeDados
{
    public class Startup(IConfiguration configuration)
    {
        private IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EFCore.Multitenant", Version = "v1" });
            });
            
            services.AddScoped<TenantData>();
            services.AddHttpContextAccessor();
            
            services.AddScoped<ApplicationContext>(provider => 
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

                var httpContext = provider.GetService<IHttpContextAccessor>()?.HttpContext;
                var tenantId = httpContext?.GetTenantId();

                var connectionString = Configuration.GetConnectionString(tenantId);
                
                if (string.IsNullOrEmpty(tenantId))
                    connectionString = configuration.GetConnectionString("default");

                optionsBuilder
                    .UseSqlServer(connectionString)
                    .LogTo(Console.WriteLine)
                    .EnableSensitiveDataLogging();

                return new ApplicationContext(optionsBuilder.Options);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EFCore.Multitenant v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            // Cria o banco de dados para rodar o exemplo
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
