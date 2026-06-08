using Microsoft.EntityFrameworkCore;
using NoteVault.DAL.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // extention
    app.UseSwaggerUI(); // extention
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // extention

app.Run();
