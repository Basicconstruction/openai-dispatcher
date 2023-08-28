using Dispatcher.Models;
using Dispatcher.Models.Requests;

namespace Dispatcher.Boot;

// 额外初始化器
public class Starter
{
    private IServiceProvider _provider;
    private KeyPoolRepository _repository;
    public Starter(IServiceProvider provider,KeyPoolRepository repository)
    {
        _repository = repository;
        _provider = provider;
    }

    // dotnet tool install --global dotnet-ef --version 3.1.1
    //  sqlcmd -S "(localdb)\MSSQLLocalDB"
    //  create database DispatcherCache
    //  go
    //  dotnet sql-cache create "server=(localdb)\MSSQLLocalDB;database=DispatcherCache" dbo DataCache
    public void Init()
    {
        using var scope = _provider.CreateScope();
        using var data = scope.ServiceProvider.GetRequiredService<DataContext>();
        _repository.Transfer(data.PoolKeys.Where(p=>p.Available==true).ToList());
    }
}