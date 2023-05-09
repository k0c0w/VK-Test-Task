namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    
    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public DateTime CreatedDate { get; set; }

    public int UserGroupId { get; set; }
    public virtual UserGroup Group { get; set; }
    
    public int UserStateId { get; set; }
    public virtual UserState State { get; set; }
}