using Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Persistence;
using Persistence.Repositories;

namespace Services.Tests;

public class UserRepositoryTest
{
    private readonly AppDbContext _ctx;
    private readonly IUserRepository _repository;
    private User DefaultUser = new() { Login = "MyAwesomeUser", Password = "SuperSecret", UserGroupId = 1, UserStateId = 1};
    
    public UserRepositoryTest()
    {
        _ctx = DbContextFactory.CreateDbContext();
        _repository = new UserRepository(_ctx);
    }


    [Fact]
    public async Task Create_UserTest()
    {
        _ctx.DeleteAllUsers();
        var user = DefaultUser;

        try
        {
            await _repository.CreateAsync(user);
        }
        catch
        {
            Assert.Fail("Unexpected fail");
        }

        var users = _ctx.Users.Any();
        Assert.True(users);
    }
    
    [Fact]
    public async Task Duplicate_UserLogin_ThrowsExceptionTest()
    {
        _ctx.DeleteAllUsers();
        var user = DefaultUser;
        var user2 = DefaultUser;
        user2.Password = "SKMflsdfz";
        user2.UserStateId = 2;
        await _repository.CreateAsync(user);

        Assert.ThrowsAsync<UserAlreadyExistsException>(async () =>  await _repository.CreateAsync(user2));
    }

    [Fact]
    public async Task GetUser_Test()
    {
        _ctx.DeleteAllUsers();
        _ctx.AddUsers(5);
        var user = _ctx.Users.First();
        var id = user.Id;
        var login = user.Login;

        var first = await _repository.GetByIdAsync(id);
        var second = await _repository.GetStrictUserByLoginAsync(login);

        Assert.Equivalent(user, first);
        Assert.Equivalent(user, second);
    }

    
}