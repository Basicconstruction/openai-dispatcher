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

    public void Init()
    {
        using var scope = _provider.CreateScope();
        using var data = scope.ServiceProvider.GetRequiredService<DataContext>();
        _repository.PoolKeys = data.PoolKeys.ToList();
    }
}