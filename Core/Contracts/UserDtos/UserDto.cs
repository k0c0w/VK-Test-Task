namespace Contracts;

public record UserDto
{
    public int Id { get; init; }
    
    public string Login { get; init; }
    
    public DateTime CreatedDate { get; init; }

    public UserGroupDto Group { get; init; }
    
    public UserStateDto State { get; init; }
}