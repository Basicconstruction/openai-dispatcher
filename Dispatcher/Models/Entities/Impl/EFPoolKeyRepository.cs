namespace Dispatcher.Models.Entities.Impl;

public class EFPoolKeyRepository: IPoolKeyRepository
{
    private DataContext _context;

    public EFPoolKeyRepository(DataContext context)
    {
        _context = context;
    }

    public IQueryable<PoolKey> PoolKeys => _context.PoolKeys;
    public void SavePoolKey(PoolKey poolKey)
    {
        _context.SaveChanges();
    }

    public void CreatePoolKey(PoolKey poolKey)
    {
        _context.Add(poolKey);
        _context.SaveChanges();
    }

    public void DeletePoolKey(PoolKey poolKey)
    {
        _context.Remove(poolKey);
        _context.SaveChanges();
    }
}