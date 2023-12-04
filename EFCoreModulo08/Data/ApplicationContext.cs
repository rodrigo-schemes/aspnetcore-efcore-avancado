using System.Reflection;
using EFCoreModulo08.Configurations;
using EFCoreModulo08.Conversores;
using EFCoreModulo08.Domain;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

namespace EFCoreModulo08.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Conversor> Conversores { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Estado> Estados { get; set; }
    public DbSet<Ator> Atores { get; set; }
    public DbSet<Filme> Filmes { get; set; }
    public DbSet<Documento> Documentos { get; set; }
    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<Instrutor> Instrutores { get; set; }
    public DbSet<Aluno> Alunos { get; set; }

    public DbSet<Loja> Lojas { get; set; }
    public DbSet<Site> Sites { get; set; }
    public DbSet<Fisica> Fisicas { get; set; }
    
    public DbSet<Dictionary<string, object>> Configuracoes => Set<Dictionary<string, object>>("Configuracoes");

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var strConnection = Setup.GetConnectionString();

        optionsBuilder
            .UseSqlServer(strConnection) 
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Collation a nível global
        // Sem case sensitive e não diferencia acentuação
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI"); 
        
        // 2. Collation a nível da propriedade
        // Case Sensitive e diferencia acentuação
        modelBuilder.Entity<Departamento>().Property(p=> p.Descricao).UseCollation("SQL_Latin1_General_CP1_CS_AS");

        // 3. Sequences
        modelBuilder.HasSequence<int>("MinhaSequencia", "sequencias")
            .StartsAt(1)
            .IncrementsBy(2)
            .HasMin(1)
            .HasMax(10)
            .IsCyclic();

        modelBuilder.Entity<Departamento>().Property(p => p.Id)
            .HasDefaultValueSql("NEXT VALUE FOR sequencias.MinhaSequencia");
        
        // 4. Criação de índices
        modelBuilder
            .Entity<Departamento>()
            .HasIndex(p=> new { p.Descricao, p.Ativo })
            .HasDatabaseName("idx_meu_indice_composto")
            .HasFilter("Descricao IS NOT NULL") //vai indexar somente campos que não são nulos, melhora a performance do indice
            .HasFillFactor(80) //fator de preenchimento do índice (default 8k). Deixará 20% para o SQL gerenciar o índice
            .IsUnique();
        
        // 5. Propagação de Dados
        // Quando o Entity rodar irá criar esses registros
        modelBuilder
            .Entity<Usuario>()
            .HasData(new Usuario
            {
                Id = 1,
                Nome = "ADM"
            }, new Usuario
            {
                Id = 2,
                Nome = "Guest"
            });
        
        // 6. Esquemas
        modelBuilder.HasDefaultSchema("cadastros"); 
        modelBuilder.Entity<Estado>().ToTable("Estados", "SegundoEsquema");
        
        // 7. Conversor de Valor
        // Neste exemplo irá converter o Enum para String para gravar no DB
        //var conversao = new ValueConverter<Versao, string>(p => p.ToString(), p => (Versao)Enum.Parse(typeof(Versao), p));
        var conversao = new EnumToStringConverter<Versao>();
        
        modelBuilder.Entity<Conversor>()
            .Property(p => p.Versao)
            .HasConversion(conversao);
        
        // 8. Conversor de Valor customizado
        modelBuilder.Entity<Conversor>()
            .Property(p => p.Status)
            .HasConversion(new ConversorCustomizado());
        
        // 9. Shadow Property
        // Vai criar um campo na tabela sem estar na Model
        modelBuilder.Entity<Departamento>().Property<DateTime>("UltimaAtualizacao");
        
        // 10. Property Bags
        // Criar uma tabela de chave e valor sem entidade mapeada
        modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Configuracoes", b =>
        {
            b.Property<int>("Id");

            b.Property<string>("Chave")
                .HasColumnType("VARCHAR(40)")
                .IsRequired();

            b.Property<string>("Valor")
                .HasColumnType("VARCHAR(255)")
                .IsRequired();
        });
        
        // Aplicar configurações do modelo
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}