namespace Dispatcher.Models.Requests;

// 内存表
public class KeyPoolRepository
{
    public List<PoolKey> PoolKeys
    {
        get;
        set;
    } = new List<PoolKey>();

    public void RemovePoolKey(PoolKey poolKey)
    {
        PoolKeys.Remove(poolKey);
    }
    public List<PoolKey> CanNotUseKeys
    {
        get;
        set;
    } = new List<PoolKey>();
    public int Count => PoolKeys.Count;

    public void Clear()
    {
        PoolKeys.Clear();
    }

    public void Add(PoolKey poolKey)
    {
        var _poolkey = PoolKeys.FirstOrDefault(p => p.PoolKeyId == poolKey.PoolKeyId);
        if (_poolkey == null)
        {
            PoolKeys.Add(poolKey);
        }
        else
        {
            _poolkey.CopyFrom(poolKey);
        }
    }


}