using Domain.Entities;

namespace Contracts;

public record UserStateDto(int Id, State Code, string Description);
