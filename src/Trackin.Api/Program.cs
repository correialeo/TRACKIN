using System.Reflection;
using dotenv.net;
using Microsoft.OpenApi.Models;
using Trackin.Api;
using Trackin.Application.Interfaces;
using Trackin.Application.Services;
using Trackin.Domain.Interfaces;
using Trackin.Infrastructure.Context;
using Trackin.Infrastructure.Persistence.Repositories;
using Trackin.Infrastructure.Persistence.Repositories.Mongo;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using MongoDB.Driver;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

DotEnv.Load();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey não configurada!");
var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero 
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("ADMINISTRADOR"));

    options.AddPolicy("AdminOrManager", policy =>
        policy.RequireRole("ADMINISTRADOR", "GERENTE"));

    options.AddPolicy("CommonOrManager", policy =>
        policy.RequireRole("COMUM", "GERENTE"));
});

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API TRACKIN - MOTTU",
        Description = "O Swagger documenta os endpoints da API TRACKIN - MOTTU.",
        Contact = new OpenApiContact() { Name = "Leandro Correia", Email = "rm556203@fiap.com.br" }
    });

    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando Bearer scheme. Digite 'Bearer' [espaço] e então seu token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    x.IncludeXmlComments(xmlPath);


});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

Settings.Initialize(builder.Configuration);
string connectionString = Settings.GetConnectionString();

// Configuração do SQL Server
builder.Services.AddDbContext<TrackinContext>(options =>
    {
        options.UseSqlServer(connectionString);
    }
);

// Configuração do MongoDB
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDB");
var mongoDatabaseName = builder.Configuration["MongoDB:DatabaseName"] ?? "TrackinDB";

builder.Services.AddSingleton<IMongoClient>(provider => new MongoClient(mongoConnectionString));

builder.Services.AddScoped<TrackinMongoContext>(provider =>
{
    var client = provider.GetRequiredService<IMongoClient>();
    var mongoUrl = MongoUrl.Create(mongoConnectionString);
    var database = client.GetDatabase(mongoUrl.DatabaseName);
    return new TrackinMongoContext(database);
});


// Health Checks
builder.Services.AddHealthChecks()
    .AddMongoDb(
        mongodbConnectionString: mongoConnectionString,
        name: "mongodb", 
        tags: new[] { "ready", "mongodb" }
    )
    .AddDbContextCheck<TrackinContext>(
        name: "sqlserver",
        tags: new[] { "ready", "sqlserver" }
    );

builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IRFIDService, RFIDService>();
builder.Services.AddScoped<IPatioService, PatioService>();
builder.Services.AddScoped<IZonaPatioService, ZonaPatioService>();
builder.Services.AddScoped<ISensorRFIDService, SensorRFIDService>();
builder.Services.AddScoped<IPatioValidacaoService, PatioValidacaoService>();
builder.Services.AddScoped<IPatioMetricasService, PatioMetricasService>();

//Autenticação e Autorização
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Repositórios SQL Server (EF Core)
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IMotoRepository, MotoRepository>();
builder.Services.AddScoped<ISensorRFIDRepository, SensorRFIDRepository>();
builder.Services.AddScoped<IEventoMotoRepository, EventoMotoRepository>();
builder.Services.AddScoped<ILocalizacaoMotoRepository, LocalizacaoMotoRepository>();
builder.Services.AddScoped<IPatioRepository, PatioRepository>();
builder.Services.AddScoped<IZonaPatioRepository, ZonaPatioRepository>();
builder.Services.AddScoped<IMotoImagemService, MotoImagemService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Repositórios MongoDB
builder.Services.AddScoped<IMotoRepository, MotoMongoRepository>();
builder.Services.AddScoped<IPatioRepository, PatioMongoRepository>();


WebApplication app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<TrackinContext>();
        
        Console.WriteLine("🔄 Aplicando migrations no banco de dados...");
        
        db.Database.Migrate();
        
        Console.WriteLine("✅ Migrations aplicadas com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erro ao aplicar migrations: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpMetrics();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapMetrics();
app.MapControllers();

// Health Check endpoints
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description,
                duration = entry.Value.Duration.TotalMilliseconds
            }),
            totalDuration = report.TotalDuration.TotalMilliseconds
        };
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
    }
});

app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false
});

app.Run();
