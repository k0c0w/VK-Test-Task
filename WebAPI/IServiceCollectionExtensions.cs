using Domain.Repositories;
using Persistence.Repositories;
using Services;
using Services.Abstractions;

namespace WebAPI;

public static class IServiceCollectionExtensions
{
    public static void AddUserService(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserGroupRepository, UserGroupRepository>();
        services.AddScoped<IUserStateRepository, UserStateRepository>();
    }
}