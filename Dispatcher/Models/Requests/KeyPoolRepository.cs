namespace Dispatcher.Models.Requests;

// 内存表
public class KeyPoolRepository
{
    private readonly object _o = new();

    public List<PoolKey> OpenPoolKeys
    {
        get
        {
            lock (_o)
            {
                return _poolKeys;
            }
        }
    }

    public void Transfer(List<PoolKey>? poolKeys)
    {
        OpenPoolKeys.Clear();
        if (poolKeys == null)
        {
            return;
        }

        OpenPoolKeys.AddRange(poolKeys);
    }
    private volatile List<PoolKey> _poolKeys = new();

    public void RemovePoolKey(PoolKey poolKey)
    {
        OpenPoolKeys.Remove(poolKey);
    }
    // public List<PoolKey> CanNotUseKeys { get; set; } = new();
    public int Count => OpenPoolKeys.Count;

    // public void Clear()
    // {
    //     PoolKeys?.Clear();
    // }

    public void Add(PoolKey key)
    {
        var poolKey = OpenPoolKeys.FirstOrDefault(p => p.PoolKeyId == key.PoolKeyId);
        if (poolKey == null)
        {
            OpenPoolKeys.Add(key);
        }
        else
        {
            poolKey.CopyFrom(poolKey);
        }
    }


}