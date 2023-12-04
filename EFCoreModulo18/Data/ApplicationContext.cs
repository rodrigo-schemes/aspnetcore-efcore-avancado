using EFCoreModulo18.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCoreModulo18.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<Departamento> Departamentos {get;set;}
    public DbSet<Colaborador> Colaboradores {get;set;}
}