using System.Net;
using System.Text;
using System.Text.Json;
using Trackin.Application.DTOs;
using Trackin.Domain.Enums;

namespace Trackin.Tests.Integration;

public class MotoControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task GetMotos_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/moto");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetMotoPorId_ComIdInexistente_RetornaNotFound()
    {
        // Arrange
        var idInexistente = 99999;

        // Act
        var response = await _client.GetAsync($"/api/moto/{idInexistente}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetMotosPorPatio_ReturnsSuccessStatusCode()
    {
        // Arrange
        var patioId = 1;

        // Act
        var response = await _client.GetAsync($"/api/moto/patio/{patioId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task EditarMoto_ComDadosValidos_RetornaSuccessStatusCode()
    {
        // Arrange - Criar pátio primeiro
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

        var contentPatio = new StringContent(
            JsonSerializer.Serialize(novoPatio),
            Encoding.UTF8,
            "application/json");

        var responsePatio = await _client.PostAsync("/api/patio", contentPatio);
        var patio = await JsonSerializer.DeserializeAsync<dynamic>(
            await responsePatio.Content.ReadAsStreamAsync());
        var patioId = (long)patio.GetProperty("id").GetInt64();

        // Arrange - Criar moto
        var novaMoto = new EditarMotoDTO
        {
            PatioId = patioId,
            Placa = "ABC1234",
            Modelo = ModeloMoto.MOTTU_SPORT,
            Ano = 2023,
            RFIDTag = "RFID123"
        };

        var contentMoto = new StringContent(
            JsonSerializer.Serialize(novaMoto),
            Encoding.UTF8,
            "application/json");

        var responseMoto = await _client.PostAsync("/api/moto", contentMoto);
        var moto = await JsonSerializer.DeserializeAsync<dynamic>(
            await responseMoto.Content.ReadAsStreamAsync());
        var motoId = (long)moto.GetProperty("id").GetInt64();

        // Arrange - Editar moto
        var editarMotoDTO = new EditarMotoDTO
        {
            PatioId = patioId,
            Placa = "XYZ9876",
            Modelo = ModeloMoto.MOTTU_SPORT,
            Ano = 2024,
            RFIDTag = "RFID456"
        };

        var contentEditar = new StringContent(
            JsonSerializer.Serialize(editarMotoDTO),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PutAsync($"/api/moto/{motoId}", contentEditar);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}