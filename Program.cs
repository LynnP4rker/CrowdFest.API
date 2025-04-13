using Microsoft.EntityFrameworkCore;
using CrowdFest.API.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCrowdFestDbContext(builder.Configuration, "DefaultConnection");
builder.Services.RegisterServices();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.Run();

