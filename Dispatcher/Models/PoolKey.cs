using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Dispatcher.Models;

public class PoolKey
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long PoolKeyId
    {
        get;
        set;
    }
    // 数据库中存储的密文
    public string? Cipher
    {
        get;
        set;
    }

    public string? Hosts
    {
        get;
        set;
    }
    // 可用的代理
    [NotMapped]
    public List<string>? AvailableHosts
    {
        get {
            if (Hosts == null)
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<List<string>>(Hosts);
            }
        }
        set => Hosts = JsonConvert.SerializeObject(value);
    }
    // 手动输入的主机，因为如果需要存储多个主机地址，会进行编码，无法手动更改
    // 所以这个优先级更高
    public string? HandHosts
    {
        get;
        set;
    }

    // 使用这个密钥的优先级,会在更高级的调度算法中使用
    public int? Priority
    {
        get;
        set;
    }

    public bool? Available
    {
        get;
        set;
    }

    public void CopyFrom(PoolKey poolKey)
    {
        Hosts = poolKey.Hosts;
        Priority = poolKey.Priority;
        Available = poolKey.Available;
    }

    public override string ToString()
    {
        return $"PoolKeyId: {PoolKeyId}, Cipher: {Cipher}, Hosts: {Hosts}, HandHosts: {HandHosts}, Priority: {Priority}, Available: {Available}";
    }

    public PoolKey Clone()
    {
        return new PoolKey()
        {
            PoolKeyId = PoolKeyId,
            Cipher = Cipher,
            Hosts = Hosts,
            HandHosts = HandHosts,
            Available = Available,
            Priority = Priority
        };
    }
}