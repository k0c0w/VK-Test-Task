using Domain.Entities;

namespace Domain.Repositories;

public interface IUserGroupRepository
{ 
    Task<UserGroup> GetGroupByCodeAsync(Role role);
    
    Task<UserGroup> GetGroupByIdAsync(int id);
}