namespace Contracts;

public class CreateUserDto
{
    public string Login { get; init; }
    
    public string Password { get; init; }
    
    public int GroupId { get; init; }

    public override string ToString()
    {
        return $"({Login};{GroupId})";
    }
}