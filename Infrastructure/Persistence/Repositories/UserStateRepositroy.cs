using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class UserStateRepository : BaseRepositoryWithDbContext, IUserStateRepository
{
    public UserStateRepository(AppDbContext context) : base(context) {}

    public async Task<UserState> GetStateAsync(State state)
    {
        var userState = await _ctx.UserStates.FirstOrDefaultAsync(x => x.Status == state);
        if (userState == null)
            throw new ObjectNotFoundException($"No {nameof(UserState)} with {nameof(UserState.Status)} == {state}");
        return userState;
    }
}