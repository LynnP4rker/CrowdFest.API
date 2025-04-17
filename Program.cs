using Microsoft.EntityFrameworkCore;
using CrowdFest.API.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddCrowdFestDbContext(builder.Configuration, "DefaultConnection");
builder.Services.RegisterServices();
builder.Services.AddAuthentication();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen(
    setupAction =>
{
    setupAction.AddSecurityDefinition("CityInfoApiBearerAuth", new ()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to access this API"
    });

    setupAction.AddSecurityRequirement(new ()
    {
        {
            new ()
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "CityInfoApiBearerAuth"
                }
            },
            new List<string>()
        }
    });
});
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Convert.FromBase64String(builder.Configuration["Authentication:SecretForKey"]))
        };
    }
);
builder.Services.AddOpenApi();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{   app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

