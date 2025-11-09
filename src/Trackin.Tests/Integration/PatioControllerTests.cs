using System.Net;
using System.Text;
using System.Text.Json;
using Trackin.Application.DTOs;

namespace Trackin.Tests.Integration;

public class PatioControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task GetPatios_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/patio");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreatePatio_WithValidData_ReturnsCreatedResponse()
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

        var content = new StringContent(
            JsonSerializer.Serialize(novoPatio),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/patio", content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}