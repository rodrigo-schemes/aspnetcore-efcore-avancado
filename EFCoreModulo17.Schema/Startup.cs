using EFCoreModulo17.Schema.Data;
using EFCoreModulo17.Schema.Data.Interceptors;
using EFCoreModulo17.Schema.Data.ModelFactory;
using EFCoreModulo17.Schema.Middlewares;
using EFCoreModulo17.Schema.Provider;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.OpenApi.Models;

namespace EFCoreModulo17.Schema
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
            
            // Estrategia 2 - Schema
            services.AddScoped<StrategySchemaInterceptor>();
            services.AddDbContext<ApplicationContext>((_,options)=>
            {
                options
                    .UseSqlServer(Setup.GetConnectionString())
                    .LogTo(Console.WriteLine)
                    .ReplaceService<IModelCacheKeyFactory, StrategySchemaModelCacheKey>()
                    .EnableSensitiveDataLogging();

                // Segunda opção, através de Replace
                // var interceptor = provider.GetRequiredService<StrategySchemaInterceptor>();
                // options.AddInterceptors(interceptor);
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
