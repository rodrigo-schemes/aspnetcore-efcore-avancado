using EFCoreModulo18.Data;
using EFCoreModulo18.Data.Repositories;
using EFCoreModulo18.Domain;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace EFCoreModulo18;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddNewtonsoftJson(options => 
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "EFCore.UowRepository", Version = "v1" });
        });

        services.AddDbContext<ApplicationContext>(p=>
            p.UseSqlServer(Setup.GetConnectionString()));

        services.AddScoped<IUnitOfWork,UnitOfWork>();
        services.AddScoped<IDepartamentoRepository,DepartamentoRepository>();
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EFCore.UowRepository v1"));
        }

        InicializarBaseDeDados(app);

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    private void InicializarBaseDeDados(IApplicationBuilder app)
    {
        using var db = app
            .ApplicationServices
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<ApplicationContext>();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
            
        db.Departamentos.AddRange(Enumerable.Range(1,10)
            .Select(p=> new Departamento
            {
                Descricao = $"Departamento - {p}",
                Colaboradores = Enumerable.Range(1,10)
                    .Select(x=> new Colaborador
                    {
                        Nome = $"Colaborador: {x}/{p}"
                    }).ToList()
            }));

        db.SaveChanges();
    }
}