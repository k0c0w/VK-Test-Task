using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Services.Tests;

public static class DbContextFactory
{
    public static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;
        var context = new AppDbContext(options);
        AddDefaultData(context);
        return context;
    }

    private static void AddDefaultData(AppDbContext context)
    {
        context.UserGroups.AddRange(
            new UserGroup
        {
            Role = Role.Admin,
            Description = "Admin",
        }, new UserGroup
            {
                Role = Role.User,
                Description = "User"
            });
        context.UserStates.AddRange(new UserState { Status = State.Active, Description = "Active" },
            new UserState { Status = State.Blocked, Description = "Blocked" });
        context.SaveChanges();
    }
}

public static class AppDbContextExtensions
{
    public static void DeleteAllUsers(this AppDbContext context)
    {
        var users = context.Users.ToList();
        context.Users.RemoveRange(users);
        context.SaveChanges();
    }
    
    public static void AddUsers(this AppDbContext _ctx, int amount)
    {
        _ctx.AddRange( Enumerable.Range(0, amount).Select(x => new User
        {
            Login = Guid.NewGuid().ToString(),
            Password = "dfgdfg",
            UserStateId = 1,
            UserGroupId = 1
        }));
        _ctx.SaveChanges();
    }
}