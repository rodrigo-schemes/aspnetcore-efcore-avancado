using EFCoreModulo20.Data;
using EFCoreModulo20.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EFCoreModulo20;

public class InMemoryTest
{
    [Fact]
    public void Deve_inserir_um_departamento()
    {
        // Arrange
        var departamento = new Departamento
        {
            Descricao = "Tecnologia",
            DataCadastro = DateTime.Now
        };

        // Setup
        var context = CreateContext();
        context.Departamentos.Add(departamento);

        // Act
        var inseridos = context.SaveChanges();

        // Assert
        Assert.Equal(1, inseridos);
    }

    [Fact]
    public void Nao_implementado_funcoes_de_datas_para_o_provider_inmemory()
    {
        // Arrange
        var departamento = new Departamento
        {
            Descricao = "Tecnologia",
            DataCadastro = DateTime.Now
        };

        // Setup
        var context = CreateContext();
        context.Departamentos.Add(departamento);

        // Act
        var inseridos = context.SaveChanges();

        // Assert - nem todos Métodos LINQ possuem tradução para o InMemory
        Action action = () => context
            .Departamentos
            .FirstOrDefault(p=> EF.Functions.DateDiffDay(DateTime.Now, p.DataCadastro) > 0);

        Assert.Throws<InvalidOperationException>(action);
    }

    private static ApplicationContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("InMemoryTest")
            .Options;

        return new ApplicationContext(options);    
    }
}