namespace Dispatcher.Models.Entities;

public interface IPoolKeyRepository
{
    IQueryable<PoolKey> PoolKeys
    {
        get;
    }

    void SavePoolKey(PoolKey poolKey);
    void CreatePoolKey(PoolKey poolKey);
    void DeletePoolKey(PoolKey poolKey);
}