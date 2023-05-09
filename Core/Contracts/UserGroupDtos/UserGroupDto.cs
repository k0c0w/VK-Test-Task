using Domain.Entities;

namespace Contracts;

public record UserGroupDto(int Id, Role Code, string Description);
