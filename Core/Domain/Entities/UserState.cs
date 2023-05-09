namespace Domain.Entities;

public class UserState
{
    public int Id { get; set; }
    
    public State Status { get; set; }
    
    public string Description { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}