using System.Globalization;
using Dispatcher.Models.Requests;
using Microsoft.Extensions.Options;

namespace Dispatcher.Models;

public class DynamicTable
{
    private volatile ServerBaseLimit _serverBaseLimit;
    public DynamicTable(IOptions<ServerBaseLimit> options)
    {
        _serverBaseLimit = options.Value;
    }
    public volatile Dictionary<string, int> KeyAvailable = new();

    public volatile Dictionary<string, int> IpAvailable = new();

    private volatile HashSet<string> _notAllowKeys = new();

    public volatile List<string> UseInfo = new();

    public void Log(string log)
    {
        UseInfo.Add(DateTime.Now.ToString(CultureInfo.InvariantCulture)+" "+log);
    }
    public void PutNotAllowKey(string key)
    {
        _notAllowKeys.Add(key);
    }

    public int ServerServeAvailable = 1;

    public const int OutLimit = 1;
    public const int IpOutLimit = 2;
    public const int KeyOutLimit = 3;
    public const int KeyIsNotAllow = 5;
    public const int Success = 4;

    public bool IsNotAllowKey(string key)
    {
        return _notAllowKeys.Contains(key);
    }
    public int Accept(string key, string ip)
    {
        if (ServerServeAvailable <= 0)
        {
            return OutLimit;
        }

        ServerServeAvailable--;

        if (_notAllowKeys.Contains(key))
        {
            return KeyIsNotAllow;
        }
        if (!IpAvailable.ContainsKey(ip))
        {
            IpAvailable[ip] = _serverBaseLimit.IpRequestLimit;
        }

        if (!KeyAvailable.ContainsKey(key))
        {
            KeyAvailable[key] = _serverBaseLimit.KeyRequestLimit;
        }

        if (IpAvailable[ip] <= 0  )
        {
            return IpOutLimit;
        }

        if (KeyAvailable[key] <= 0)
        {
            return KeyOutLimit;
        }

        IpAvailable[ip]--;
        KeyAvailable[key]--;

        return Success;
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
        _notAllowKeys.Clear();

        ServerServeAvailable = _serverBaseLimit.ServerServeLimit;
    }
}