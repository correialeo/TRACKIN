using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Trackin.API.Infrastructure.Context;
using Trackin.API.Infrastructure.Persistence.Repositories;
using Trackin.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TrackinContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
}
            );

builder.Services.AddScoped<MotoService>();
builder.Services.AddScoped<RFIDService>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IMotoRepository, MotoRepository>();

var app = builder.Build();

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
