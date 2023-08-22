using Dispatcher.Models.Requests;
using Microsoft.Extensions.Options;

namespace Dispatcher.Models;

public class DynamicTable
{
    private ServerBaseLimit _serverBaseLimit;
    public DynamicTable(IOptions<ServerBaseLimit> options)
    {
        _serverBaseLimit = options.Value;
    }
    public Dictionary<string, int> KeyAvailable
    {
        get;
        set;
    } = new Dictionary<string, int>();

    public Dictionary<string, int> IpAvailable
    {
        get;
        set;
    } = new Dictionary<string, int>();

    public int ServerServeAvailable
    {
        get;
        set;
    }

    public bool Accept(string key, string ip)
    {
        if (ServerServeAvailable <= 0)
        {
            return false;
        }

        ServerServeAvailable--;

        if (!IpAvailable.ContainsKey(ip))
        {
            IpAvailable[ip] = _serverBaseLimit.IpRequestLimit;
        }

        if (!KeyAvailable.ContainsKey(key))
        {
            KeyAvailable[key] = _serverBaseLimit.KeyRequestLimit;
        }

        if (IpAvailable[ip] <= 0 || KeyAvailable[key] <= 0)
        {
            return false;
        }

        IpAvailable[ip]--;
        KeyAvailable[key]--;

        return true;
    }

    public void Reset()
    {
        foreach (var IpPair in IpAvailable)
        {
            IpAvailable[IpPair.Key] = _serverBaseLimit.IpRequestLimit;
        }

        foreach (var keyPair in KeyAvailable)
        {
            KeyAvailable[keyPair.Key] = _serverBaseLimit.KeyRequestLimit;
        }

        ServerServeAvailable = _serverBaseLimit.ServerServeLimit;
    }
}