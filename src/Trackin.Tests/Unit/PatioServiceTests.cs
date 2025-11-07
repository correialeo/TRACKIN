using Moq;
using Trackin.Application.Services;
using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;
using Trackin.Application.DTOs;
using Trackin.Tests.Extensions;

namespace Trackin.Tests.Unit;

public class PatioServiceTests
{
    private readonly Mock<IPatioRepository> _patioRepositoryMock;
    private readonly PatioService _patioService;

    public PatioServiceTests()
    {
        _patioRepositoryMock = new Mock<IPatioRepository>();
        _patioService = new PatioService(_patioRepositoryMock.Object);
    }

    [Fact]
    public async Task CriarPatio_ComDadosValidos_DeveCriarComSucesso()
    {
        // Arrange
        var novoPatio = new CriarPatioDto
        {
            Nome = "Patio Teste",
            Endereco = "Rua do Teste, 123",
            Cidade = "Cidade Teste",
            Estado = "Estado Teste",
            Pais = "Brasil",
            DimensaoX = 100,
            DimensaoY = 100
        };

        _patioRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Patio>()))
            .Returns(Task.FromResult(new Patio(novoPatio.Nome, novoPatio.Endereco, novoPatio.Cidade, novoPatio.Estado, "Brasil", 100, 100)));

        // Act
        var resultado = await _patioService.CreatePatioAsync(novoPatio);

        // Assert
        Assert.True(resultado.Success);
        Assert.NotNull(resultado.Data);
        Assert.Equal(novoPatio.Nome, resultado.Data.Nome);
        _patioRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Patio>()), Times.Once);
    }

    [Fact]
    public async Task ObterPatio_ComIdExistente_DeveRetornarPatio()
    {
        // Arrange
        var patioId = 1;
        var patio = new Patio("Patio Teste", "Endereco Teste", "Cidade Teste", "Estado Teste", "Brasil", 100, 100);
        
        var patioComId = new Patio("Patio Teste", "Endereco Teste", "Cidade Teste", "Estado Teste", "Brasil", 100, 100);
        patioComId.SetId(patioId);
        _patioRepositoryMock.Setup(repo => repo.GetByIdAsync(patioId))
            .Returns(Task.FromResult(patioComId));

        // Act
        var resultado = await _patioService.GetPatioByIdAsync(patioId);

        // Assert
        Assert.True(resultado.Success);
        Assert.NotNull(resultado.Data);
        Assert.Equal(1, resultado.Data.Id);
        _patioRepositoryMock.Verify(repo => repo.GetByIdAsync(patioId), Times.Once);
    }
}