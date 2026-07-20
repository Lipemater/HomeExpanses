using System.Text.Json.Serialization;
using HomeExpanses.Api.ErrorHandling;
using HomeExpanses.Application;
using HomeExpanses.Infrastructure;
using HomeExpanses.Infrastructure.Persistence;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        /*
         * Faz a API receber e devolver:
         * "Despesa" e "Receita"
         * Em vez de:
         * 1 e 2
         */
         options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(
    builder.Configuration);

builder.Services.AddCors(Options =>
{
    Options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("Frontend");

app.MapControllers();

/*
 * Cria o banco e aplica migrations pendentes.
 */
 await app.Services.ApplyMigrationsAsync();

app.Run();
