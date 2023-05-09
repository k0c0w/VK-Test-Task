namespace Domain.Entities;

public class UserGroup
{
    public int Id { get; set; }
    
    public Role Role { get; set; }
    
    public string Description { get; set; }
    
    public virtual ICollection<User> Users { get; } = new List<User>();
}