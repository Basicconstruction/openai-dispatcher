using Microsoft.EntityFrameworkCore;

namespace Dispatcher.Models;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> opts) : base(opts)
    {

    }

    public DbSet<KeyUser> Users
    {
        get;
        set;
    }

    public DbSet<OpenKey> OpenKeys
    {
        get;
        set;
    }

    public DbSet<PoolKey> PoolKeys
    {
        get;
        set;
    }
}