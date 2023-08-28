namespace Dispatcher.Models.Requests;

// 内存表
public class KeyPoolRepository
{
    public volatile List<PoolKey>? PoolKeys = new();

    public void RemovePoolKey(PoolKey poolKey)
    {
        PoolKeys?.Remove(poolKey);
    }
    // public List<PoolKey> CanNotUseKeys { get; set; } = new();
    public int Count => PoolKeys?.Count??0;

    // public void Clear()
    // {
    //     PoolKeys?.Clear();
    // }

    public void Add(PoolKey key)
    {
        if (PoolKeys != null)
        {
            var poolKey = PoolKeys.FirstOrDefault(p => p.PoolKeyId == key.PoolKeyId);
            if (poolKey == null)
            {
                PoolKeys.Add(key);
            }
            else
            {
                poolKey.CopyFrom(poolKey);
            }
        }
    }


}