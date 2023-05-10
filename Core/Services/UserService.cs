using System.Text;
using Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Services.Abstractions;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserStateRepository _userStateRepository;
    private readonly IUserGroupRepository _userGroupRepository;

    public UserService(IUserRepository userRepository, IUserStateRepository userStateRepository, 
        IUserGroupRepository userGroupRepository)
    {
        _userRepository = userRepository;
        _userStateRepository = userStateRepository;
        _userGroupRepository = userGroupRepository;
    }

    public async Task CreateUserAsync(CreateUserDto user)
    {
        var existingUser = await _userRepository.GetStrictUserByLoginAsync(user.Login);
        if (existingUser != null) throw new UserAlreadyExistsException();

        var userGroup = await _userGroupRepository.GetGroupByIdAsync(user.GroupId);
        if (userGroup.Role == Role.Admin)
        {
            var admins = await _userRepository.GetManyByRoleAsync(Role.Admin);
            if ( admins.Any()) throw new InvalidOperationException($"Can not create user {user}.");
        }

        await SaveNewUserAsync(user);
    }

    public async Task DeleteUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return;

        var nonActiveState = await _userStateRepository.GetStateAsync(State.Blocked);
        user.State = nonActiveState;
        await _userRepository.UpdateAsync(user);
    }

    public async Task<UserDto> GetUserInfoByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        var group = user.Group;
        var state = user.State;
        return new UserDto
        {
            Id = id,
            Login = user.Login,
            CreatedDate = user.CreatedDate,
            Group = new UserGroupDto(group.Id, group.Role, group.Description),
            State = new UserStateDto(state.Id, state.Status, state.Description)
        };
    }

    public async Task<IEnumerable<UserDto>> GetManyUserInfosAsync(int offset = 0, int limit = 32)
    {
        var users = await _userRepository.GetManyAsync(offset, limit);
        return users.Select(x => new UserDto
        {
            Id = x.Id,
            Login = x.Login,
            CreatedDate = x.CreatedDate,
            Group = new UserGroupDto(x.Id, x.Group.Role, x.Group.Description),
            State = new UserStateDto(x.Id, x.State.Status, x.State.Description)
        });
    }

    public async Task<bool> AuthenticateAsync(string login, string password)
    {
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password)) return false;
        
        var user = await _userRepository.GetStrictUserByLoginAsync(login);
        if(user != null)
            return user.Password == GetPasswordHash(password);
        
        return false;
    }

    private async Task SaveNewUserAsync(CreateUserDto user)
    {
        var userState = await _userStateRepository.GetStateAsync(State.Active);
        var userGroup = await _userGroupRepository.GetGroupByIdAsync(user.GroupId);
        var newUser = new User
        {
            Login = user.Login,
            Group = userGroup,
            State = userState,
            CreatedDate = DateTime.Now,
            Password =  GetPasswordHash(user.Password),
        };
        
        await _userRepository.CreateAsync(newUser);
        //to imitate hard logic
        await Task.Delay(TimeSpan.FromSeconds(5));
    }

    private string GetPasswordHash(string password)
    {
        return password.GetHashCode().ToString();
    }
}