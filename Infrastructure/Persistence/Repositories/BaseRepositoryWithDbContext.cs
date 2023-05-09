namespace Persistence.Repositories;

public abstract class BaseRepositoryWithDbContext
{
    protected readonly AppDbContext _ctx;

    public BaseRepositoryWithDbContext(AppDbContext context)
    {
        _ctx = context;
    }
}