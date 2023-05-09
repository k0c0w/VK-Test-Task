using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class UserGroupRepository : BaseRepositoryWithDbContext, IUserGroupRepository
{
    public UserGroupRepository(AppDbContext context) : base(context) { }

    public async Task<UserGroup> GetGroupByCodeAsync(Role role)
    {
        var group = await _ctx.UserGroups.FirstOrDefaultAsync(x => x.Role == role);
        return ThrowIfNull(group);
    }

    public async Task<UserGroup> GetGroupByIdAsync(int id)
    {
        var group = await _ctx.UserGroups.FirstOrDefaultAsync(x => x.Id == id);
        return ThrowIfNull(group);
    }

    private UserGroup ThrowIfNull(UserGroup group)
    {
        if (group == null)
            throw new ObjectNotFoundException();
        return group;
    }
}