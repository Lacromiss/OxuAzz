using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using OxuAzz.Context;
using OxuAzz.Dtos;
using OxuAzz.Dtos.NewDto;
using OxuAzz.Models;
using OxuAzz.Profils;
using OxuAzz.Validations.News.News;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddAutoMapper(typeof(CategoryMapProfile));
builder.Services.AddAutoMapper(typeof(NewMapProfile));
builder.Services.AddControllers()?.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<NewDtoValidation>());





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
