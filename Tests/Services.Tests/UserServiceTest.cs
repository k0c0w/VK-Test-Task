using Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;
using Services.Abstractions;

namespace Services.Tests;

public class UserServiceTest
{
    private IUserService _service;
    private AppDbContext _ctx;

    public UserServiceTest()
    {
         _ctx = DbContextFactory.CreateDbContext();
        var userRep = new UserRepository(_ctx);
        var stateRep = new UserStateRepository(_ctx);
        var groupRep = new UserGroupRepository(_ctx);
        _service = new UserService(userRep, stateRep, groupRep);
    }
    
    
    [Fact]
    public async Task Create_UserTest()
    {
        _ctx.DeleteAllUsers();
        var user = new CreateUserDto { Login = "looka", Password = "supersecret", GroupId = 1 };

        try
        {
            await _service.CreateUserAsync(user);
        }
        catch
        {
            Assert.Fail("User was not created");
        }

        var created = _ctx.Users.Include(x => x.State).FirstOrDefault();
        Assert.True(created != null);
        Assert.Equal(created.Login, user.Login);
        Assert.Equal(created.UserGroupId, user.GroupId);
        Assert.Equal(created.State.Status, State.Active);
    }
    
    [Fact]
    public async Task Create_AdminAndUserTest()
    {
        _ctx.DeleteAllUsers();
        var adminId = _ctx.UserGroups.First(x => x.Role == Role.Admin).Id;
        var userId = _ctx.UserGroups.First(x => x.Role == Role.User).Id;
        var admin1 = new CreateUserDto { Login = "admin", Password = "sdfsdfsdfs", GroupId = adminId };
        var user = new CreateUserDto { Login = "secondadmin", Password = "sdfsdfsdfs", GroupId = userId };

        await _service.CreateUserAsync(admin1);
        await _service.CreateUserAsync(user);

        Assert.True(_ctx.Users.Count() == 2);
    }

    [Fact]
    public async Task CanNotCreate_ManyAdminsTest()
    {
        _ctx.DeleteAllUsers();
        var adminId = _ctx.UserGroups.First(x => x.Role == Role.Admin).Id;
        var admin1 = new CreateUserDto { Login = "admin", Password = "sdfsdfsdfs", GroupId = adminId };
        var admin2 = new CreateUserDto { Login = "secondadmin", Password = "sdfsdfsdfs", GroupId = adminId };

        await _service.CreateUserAsync(admin1);

        Assert.ThrowsAnyAsync<Exception>(async () => await _service.CreateUserAsync(admin2));
    }
    
    [Fact]
    public async Task CanNotCreate_WithWrongGroupIdTest()
    {
        _ctx.DeleteAllUsers();
        var user = new CreateUserDto { Login = "admin", Password = "sdfsdfsdfs", GroupId = -1 };


        Assert.ThrowsAnyAsync<Exception>(async () => await _service.CreateUserAsync(user));
    }

    [Fact]
    public async Task CreateOnlyOne_WhenTwoTriesCreate()
    {
        _ctx.DeleteAllUsers();
        var userId = _ctx.UserGroups.First(x => x.Role == Role.User).Id;
        var adminId = _ctx.UserGroups.First(x => x.Role == Role.Admin).Id;
        var user1 = new CreateUserDto { Login = "user", Password = "Dsfsdfsdf", GroupId = userId };
        var user2 = new CreateUserDto { Login = "user", Password = "sdfdsfsdfsdfs", GroupId = adminId };
        var task1 = new Task(async () => await _service.CreateUserAsync(user1));
        var task2 = new Task(async () => await _service.CreateUserAsync(user2));

        task1.Start();
        task2.Start();
        Task.WaitAll(task1, task2);
        
        Assert.True(_ctx.Users.Count() == 1);
    }
}