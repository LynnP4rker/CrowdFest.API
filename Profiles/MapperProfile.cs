using AutoMapper;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;

public class MapperProfile: Profile
{
    public MapperProfile()
    {
        CreateMap<PlannerDto, PlannerEntity>();
        CreateMap<PlannerEntity, PlannerDto>();

        CreateMap<EventDto, EventEntity>();
        CreateMap<EventEntity, EventDto>();

        CreateMap<GroupDto, GroupEntity>();
        CreateMap<GroupEntity, GroupDto>();

        CreateMap<LocationDto, LocationEntity>();
        CreateMap<LocationEntity, LocationDto>();

        CreateMap<PlannerGroupDto, PlannerGroupEntity>();
        CreateMap<PlannerGroupEntity, PlannerGroupDto>();

        CreateMap<PostDto, PostEntity>();
        CreateMap<PostEntity, PostDto>();

        CreateMap<LocationDto, LocationEntity>();
        CreateMap<LocationEntity, LocationDto>();

        CreateMap<ThemeDto, ThemeEntity>();
        CreateMap<ThemeEntity, ThemeDto>();

        CreateMap<OrganizationDto, OrganizationEntity>();
        CreateMap<OrganizationEntity, OrganizationDto>();

        CreateMap<RegisterPlannerDto, PlannerEntity>();
        CreateMap<RegisterOrganizationDto, OrganizationEntity>();
    }
}