using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace WebAPI.Tests;

public class TestApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbDescriptor = services.SingleOrDefault(
                descriptor => descriptor.ServiceType == typeof(DbContextOptions<AppDbContext>));
            services.Remove(dbDescriptor!);
            
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            
            var nameTestDb = Guid.NewGuid().ToString();
            services.AddDbContext<AppDbContext>(
                optionsBuilder => optionsBuilder.UseInMemoryDatabase(nameTestDb));
            
            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            InitDataBase(db);
        }); 
    }

    private void InitDataBase(AppDbContext context)
    {
        context.UserGroups.AddRange(new UserGroup { Role = Role.Admin, Description = "Admin" },
            new UserGroup { Description = "User", Role = Role.User });
        context.UserStates.AddRange(new UserState { Status = State.Active, Description = "Active" },
            new UserState { Description = "Blocked", Status = State.Blocked });
        context.SaveChanges();
    }
}