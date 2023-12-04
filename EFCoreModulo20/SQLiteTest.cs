using EFCoreModulo20.Data;
using EFCoreModulo20.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EFCoreModulo20;

public class SqLiteTest
{
    [Theory]
    [InlineData("Tecnologia")]
    [InlineData("Financeiro")]
    [InlineData("Departamento Pessoal")]
    public void Deve_inserir_e_consultar_um_departamento(string descricao)
    {
        // Arrange
        var departamento = new Departamento
        {
            Descricao = descricao,
            DataCadastro = DateTime.Now
        };

        // Setup
        var context = CreateContext();
        context.Database.EnsureCreated();
        context.Departamentos.Add(departamento);

        // Act
        var inseridos = context.SaveChanges();
        departamento = context.Departamentos.FirstOrDefault(p=>p.Descricao == descricao);

        // Assert
        Assert.Equal(1, inseridos);
        Assert.Equal(descricao, departamento.Descricao);
    }

    private ApplicationContext CreateContext()
    {
        var conexao = new SqliteConnection("Datasource=:memory:");
        conexao.Open();

        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseSqlite(conexao)
            .Options;

        return new ApplicationContext(options);    
    }
}