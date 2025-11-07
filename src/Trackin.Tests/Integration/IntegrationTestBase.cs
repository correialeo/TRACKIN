using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using Trackin.Infrastructure.Context;
using Trackin.Api;

namespace Trackin.Tests.Integration;

public class IntegrationTestBase : IDisposable
{
    protected readonly WebApplicationFactory<Program> _factory;
    protected readonly HttpClient _client;

    public IntegrationTestBase()
    {
        _factory = new WebApplicationFactory<Program>();
        _factory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Configurar banco de dados em memória para testes
                var dbDescriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<TrackinContext>));

                if (dbDescriptor != null)
                {
                    services.Remove(dbDescriptor);
                }

                services.AddDbContext<TrackinContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });

                // Substituir JWT por autenticação de teste
                services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.UseSecurityTokenValidators = true;
                    options.SecurityTokenValidators.Clear();
                    options.SecurityTokenValidators.Add(new TestTokenValidator());
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<TrackinContext>();
                db.Database.EnsureCreated();
            });
        });

        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "TestToken");
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}