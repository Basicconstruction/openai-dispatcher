namespace Dispatcher.Models.Requests;

public class ServerBaseLimit
{
    // 每分钟限制
    public int ServerServeLimit
    {
        get;
        set;
    } = 200;

    public int IpRequestLimit
    {
        get;
        set;
    } = 10;

    public int KeyRequestLimit
    {
        get;
        set;
    } = 40;
}