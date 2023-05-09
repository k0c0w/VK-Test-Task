using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetManyAsync(int offset=0, int limit=32);

    Task<User> GetByIdAsync(int id);
    
    Task<User?> GetStrictUserByLoginAsync(string login);
    
    Task<IEnumerable<User>> GetManyByRoleAsync(Role role, int offset=0, int limit=32);

    Task CreateAsync(User user);

    Task UpdateAsync(User user);
}