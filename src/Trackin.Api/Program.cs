using System.Reflection;
using dotenv.net;
using Microsoft.OpenApi.Models;
using Trackin.Api;
using Trackin.Application.Interfaces;
using Trackin.Application.Services;
using Trackin.Domain.Interfaces;
using Trackin.Infrastructure.Context;
using Trackin.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

DotEnv.Load();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API TRACKIN - MOTTU",
        Description = "O Swagger documenta os endpoints da API TRACKIN - MOTTU.",
        Contact = new OpenApiContact() { Name = "Leandro Correia", Email = "rm556203@fiap.com.br" }
    });


    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    x.IncludeXmlComments(xmlPath);
});

Settings.Initialize(builder.Configuration);
string connectionString = Settings.GetConnectionString();

builder.Services.AddDbContext<TrackinContext>(options =>
    {
        options.UseSqlServer(connectionString);
    }
 );

builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IRFIDService, RFIDService>();
builder.Services.AddScoped<IPatioService, PatioService>();
builder.Services.AddScoped<IZonaPatioService, ZonaPatioService>();
builder.Services.AddScoped<ISensorRFIDService, SensorRFIDService>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IMotoRepository, MotoRepository>();
builder.Services.AddScoped<ISensorRFIDRepository, SensorRFIDRepository>();
builder.Services.AddScoped<IEventoMotoRepository, EventoMotoRepository>();
builder.Services.AddScoped<ILocalizacaoMotoRepository, LocalizacaoMotoRepository>();
builder.Services.AddScoped<IPatioRepository, PatioRepository>();
builder.Services.AddScoped<IZonaPatioRepository, ZonaPatioRepository>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
