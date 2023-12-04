using EFCoreModulo17.Tabela.Domain;
using EFCoreModulo17.Tabela.Provider;
using Microsoft.EntityFrameworkCore;

namespace EFCoreModulo17.Tabela.Data
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options, TenantData tenantData)
        : DbContext(options)
    {
        public DbSet<Person> People {get;set;}
        public DbSet<Product> Products {get;set;}
        
        public readonly TenantData TenantData = tenantData;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, Name = "Person 1", TenantId = "tenant-1"},
                new Person { Id = 2, Name = "Person 2", TenantId = "tenant-2"},
                new Person { Id = 3, Name = "Person 3", TenantId = "tenant-2"});

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Description = "Description 1", TenantId = "tenant-1"},
                new Product { Id = 2, Description = "Description 2", TenantId = "tenant-2"},
                new Product { Id = 3, Description = "Description 3", TenantId = "tenant-2"});

            // Estrategia Tabela - query global filtrando por TenantId
            modelBuilder.Entity<Person>().HasQueryFilter(p=>p.TenantId == TenantData.TenantId);
            modelBuilder.Entity<Product>().HasQueryFilter(p=>p.TenantId == TenantData.TenantId);
        }
    }
}