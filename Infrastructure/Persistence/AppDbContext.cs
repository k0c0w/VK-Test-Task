using System.Linq.Expressions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    public virtual DbSet<User> Users { get; init; }
    
    public virtual DbSet<UserGroup> UserGroups { get; init; }
    
    public virtual DbSet<UserState> UserStates { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureUsers(modelBuilder);
        ConfigureUserGroups(modelBuilder);
        ConfigureUserStates(modelBuilder);
    }

    private void ConfigureUsers(ModelBuilder builder)
    {
        SetTableName<User>(builder, "user");
        var user = builder.Entity<User>();
        HasColumnName(user, x => x.Id, "id");
        HasColumnName(user, x => x.Login, "login");
        HasColumnName(user, x => x.Password, "password");
        HasColumnName(user, x => x.CreatedDate, "created_date");
        HasColumnName(user, x => x.UserGroupId, "user_group_id");
        HasColumnName(user, x => x.UserStateId, "user_state_id");

        user.HasOne(x => x.Group)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.UserGroupId);
        user.HasOne(x => x.State)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.UserStateId);

        user.HasAlternateKey(x => x.Login);
    }

    private void ConfigureUserGroups(ModelBuilder builder)
    {
        SetTableName<UserGroup>(builder, "user_group");
        var group = builder.Entity<UserGroup>();
        HasColumnName(group, x => x.Id, "id");
        HasColumnName(group, x => x.Role, "code");
        HasColumnName(group, x => x.Description, "description");
        group.Property(x => x.Role).HasConversion(to => to.ToString(), 
                from => (Role)Enum.Parse(typeof(Role), from));
        group.HasAlternateKey(x => x.Role);

        group.HasData(new UserGroup { Id=1, Role = Role.Admin, Description = "Main hero of database." },
            new UserGroup { Id=2, Role = Role.User, Description = "Just a user." });
    }
    
    private void ConfigureUserStates(ModelBuilder builder)
    {
        SetTableName<UserState>(builder, "user_state");
        var state = builder.Entity<UserState>();
        HasColumnName(state, x => x.Id, "id");
        HasColumnName(state, x => x.Status, "code");
        HasColumnName(state, x => x.Description, "description");
        state.Property(x => x.Status).HasConversion(to => to.ToString(), 
            from => (State)Enum.Parse(typeof(State), from));
        state.HasAlternateKey(x => x.Status);
        
        state.HasData(new UserState { Id=1, Status = State.Active, Description = "User can access service." },
            new UserState { Id=2, Status = State.Blocked, Description = "Administratively deleted." });
    }
    
    private void HasColumnName<TEntity, TProperty>(EntityTypeBuilder<TEntity> entityBuilder,
        Expression<Func<TEntity, TProperty>> propertySelector, string name)
        where TEntity : class
        => entityBuilder.Property(propertySelector).HasColumnName(name);
    
    private void SetTableName<TEntity>(ModelBuilder builder, string name) where TEntity : class =>
        builder.Entity<TEntity>().ToTable(name);
}