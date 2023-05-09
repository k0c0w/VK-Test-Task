using Domain.Entities;

namespace Domain.Repositories;

public interface IUserStateRepository
{
    Task<UserState> GetStateAsync(State state);
}