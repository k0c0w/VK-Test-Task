using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class UserRepository : BaseRepositoryWithDbContext, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) {}

    public async Task<IEnumerable<User>> GetManyAsync(int offset = 0, int limit = 32)
    { 
        ThrowIfInvalidArgs(offset, limit); 
        return await _ctx.Users
            .Skip(offset)
            .Take(limit)
            .Include(x => x.State)
            .Include(x => x.Group)
            .ToListAsync();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _ctx.Users
            .Include(x => x.Group)
            .Include(x => x.State)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<User?> GetStrictUserByLoginAsync(string login)
    {
        if (string.IsNullOrEmpty(login) || string.IsNullOrWhiteSpace(login)) return null;
        return await _ctx.Users
            .FirstOrDefaultAsync(x => x.Login == login);
    }

    public async Task<IEnumerable<User>> GetManyByRoleAsync(Role role, int offset = 0, int limit = 32)
    {
        ThrowIfInvalidArgs(offset, limit);
        return await _ctx.Users
            .Skip(offset)
            .Take(limit)
            .Include(x => x.Group)
            .Where(x => x.Group.Role == role)
            .ToListAsync();
    } 

    public async Task CreateAsync(User user)
    {
        try
        {
            await _ctx.Users.AddAsync(user);
            await _ctx.SaveChangesAsync();
            //to imitate hard logic
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
        catch (DbUpdateException)
        {
            throw new UserAlreadyExistsException();
        }
    }

    public async Task UpdateAsync(User user)
    {
        _ctx.Users.Update(user);
        await _ctx.SaveChangesAsync();
    }

    private void ThrowIfInvalidArgs(int offset, int limit)
    {
        if (offset < 0 || limit < 0) 
            throw new ArgumentException("Arguments must be non-negative"); 
    }
}