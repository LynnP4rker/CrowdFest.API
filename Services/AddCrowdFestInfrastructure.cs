using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Repositories;
using Microsoft.AspNetCore.Identity;

namespace CrowdFest.API.Services;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IPlannerGroupRepository, PlannerGroupRepository>();
        services.AddScoped<IPlannerRepository, PlannerRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IThemeRepository, ThemeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVoteRepository, VoteRepository>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IEncryptionService, EncryptionService>();

        return services;
    }
}