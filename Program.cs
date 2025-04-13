using Microsoft.EntityFrameworkCore;
using CrowdFest.API.Services;

var builder = WebApplication.CreateBuilder(args);

//Future Lynn: move this to a 
builder.Services.AddDbContext<CrowdFestDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 3)) 
    ));
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

