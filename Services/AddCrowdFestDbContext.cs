using Microsoft.EntityFrameworkCore;

namespace CrowdFest.API.Services;

public static class DbContextRegistration
{
    public static IServiceCollection AddCrowdFestDbContext(
        this IServiceCollection services,
        IConfiguration configuration,
        string? connectionString)
    {
        services.AddDbContext<CrowdFestDbContext>(options => 
            options.UseMySql(
                configuration.GetConnectionString(connectionString),
                new MySqlServerVersion(new Version(8, 0, 3)) 
            )
        );

        return services;
    }
}