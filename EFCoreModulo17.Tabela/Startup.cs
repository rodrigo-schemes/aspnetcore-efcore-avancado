using EFCoreModulo17.Tabela.Data;
using EFCoreModulo17.Tabela.Middlewares;
using EFCoreModulo17.Tabela.Provider;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace EFCoreModulo17.Tabela
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EFCore.Multitenant", Version = "v1" });
            });
            
            // Middleware atribuirá o valor do Tenant a esta classe
            services.AddScoped<TenantData>();
            
            // Estratégia 1 - Tabela
            services.AddDbContext<ApplicationContext>(p =>
                p.UseSqlServer(Setup.GetConnectionString())
                    .LogTo(Console.WriteLine)
                    .EnableSensitiveDataLogging());
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

            // Middleware que captura o TenantId através da URL
            app.UseMiddleware<TenantMiddleware>();
            
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
