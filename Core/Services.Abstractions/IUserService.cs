using Contracts;

namespace Services.Abstractions;

public interface IUserService
{
    Task CreateUserAsync(CreateUserDto user);
    
    Task DeleteUserByIdAsync(int id);

    Task<UserDto> GetUserInfoByIdAsync(int id);
    
    Task<IEnumerable<UserDto>> GetManyUserInfosAsync(int offset=0, int limit=32);
    
    Task<bool> AuthenticateAsync(string login, string password);
}