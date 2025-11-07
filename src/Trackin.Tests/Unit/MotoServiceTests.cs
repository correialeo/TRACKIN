using Moq;
using Trackin.Application.Services;
using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;
using Trackin.Application.DTOs;
using Trackin.Domain.Enums;

namespace Trackin.Tests.Unit;

public class MotoServiceTests
{
    private readonly Mock<IMotoRepository> _motoRepositoryMock;
    private readonly Mock<IPatioRepository> _patioRepositoryMock;
    private readonly MotoService _motoService;

    public MotoServiceTests()
    {
        _motoRepositoryMock = new Mock<IMotoRepository>();
        _patioRepositoryMock = new Mock<IPatioRepository>();
        _motoService = new MotoService(_motoRepositoryMock.Object, _patioRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateMotoAsync_ComDadosValidos_DeveCriarComSucesso()
    {
        // Arrange
        var motoDTO = new MotoDTO
        {
            PatioId = 1,
            Placa = "ABC-1234",
            Modelo = ModeloMoto.MOTTU_SPORT,
            Ano = 2023,
            RFIDTag = "RFID123"
        };

        var patio = new Patio("Patio Teste", "Endereco Teste", "Cidade Teste", "Estado Teste", "Brasil", 100, 100);

        _patioRepositoryMock.Setup(repo => repo.GetByIdAsync(motoDTO.PatioId))
            .Returns(Task.FromResult(patio));

        _motoRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Moto>()))
            .Returns(Task.FromResult(new Moto(motoDTO.PatioId, motoDTO.Placa, motoDTO.Modelo, motoDTO.Ano, motoDTO.RFIDTag)));

        // Act
        var resultado = await _motoService.CreateMotoAsync(motoDTO);

        // Assert
        Assert.True(resultado.Success);
        Assert.NotNull(resultado.Data);
        Assert.Equal(motoDTO.Placa, resultado.Data.Placa);
        _motoRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Moto>()), Times.Once);
        _motoRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetMotoByIdAsync_ComIdExistente_DeveRetornarMoto()
    {
        // Arrange
        var motoId = 1L;
        var motoEsperada = new Moto(1, "ABC-1234", ModeloMoto.MOTTU_SPORT, 2023, "RFID123");
        
        _motoRepositoryMock.Setup(repo => repo.GetByIdAsync(motoId))
            .ReturnsAsync(motoEsperada);

        // Act
        var resultado = await _motoService.GetMotoByIdAsync(motoId);

        // Assert
        Assert.True(resultado.Success);
        Assert.NotNull(resultado.Data);
        Assert.Equal(motoEsperada.Placa, resultado.Data.Placa);
    }

    [Fact]
    public async Task GetMotoByIdAsync_ComIdInexistente_DeveRetornarErro()
    {
        // Arrange
        var motoId = 999L;
        _motoRepositoryMock.Setup(repo => repo.GetByIdAsync(motoId))
            .ReturnsAsync((Moto)null);

        // Act
        var resultado = await _motoService.GetMotoByIdAsync(motoId);

        // Assert
        Assert.False(resultado.Success);
        Assert.Null(resultado.Data);
        Assert.Equal("Moto não encontrada", resultado.Message);
    }

    [Fact]
    public async Task DeleteMotoAsync_ComIdExistente_DeveDeletarComSucesso()
    {
        // Arrange
        var motoId = 1L;
        var moto = new Moto(1, "ABC-1234", ModeloMoto.MOTTU_SPORT, 2023, "RFID123");
        
        _motoRepositoryMock.Setup(repo => repo.GetByIdAsync(motoId))
            .ReturnsAsync(moto);

        // Act
        var resultado = await _motoService.DeleteMotoAsync(motoId);

        // Assert
        Assert.True(resultado.Success);
        _motoRepositoryMock.Verify(repo => repo.RemoveAsync(moto), Times.Once);
        _motoRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }
}