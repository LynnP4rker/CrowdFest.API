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
        services.AddScoped<IPlannerAccountRepository, PlannerAccountRepository>();
        services.AddScoped<IPlannerRepository, PlannerRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IThemeRepository, ThemeRepository>();
        services.AddScoped<IVoteRepository, VoteRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IOrganizationAccountRepository, OrganizationAccountRepository>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IEncryptionService, EncryptionService>();

        return services;
    }
}