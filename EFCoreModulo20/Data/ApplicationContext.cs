using EFCoreModulo20.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreModulo20.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }
    }
}