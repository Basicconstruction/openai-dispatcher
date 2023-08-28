using Dispatcher.Models;
using Dispatcher.Models.Requests;

namespace Dispatcher.FakeGpt;

public class Syncer
{
    private DataContext? _context;
    // private IServiceProvider _provider;
    private readonly KeyPoolRepository _repository;

    public Syncer(IServiceProvider serviceProvider,KeyPoolRepository repository)
    {
        // _context = context;
        var scope = serviceProvider?.CreateScope();
        var context = scope?.ServiceProvider.GetRequiredService<DataContext>();
        _context = context;
        _repository = repository;
    }
    public void UpdateDynamicKeys()
    {
        _repository.Transfer(_context?.PoolKeys.Where(p=>p.Available==true).ToList());
        // _repository.Transfer(_context?.PoolKeys.Where(p=>p.Available==true).ToList());
        // _repository.PoolKeys = _context?.PoolKeys.Where(p=>p.Available==true).ToList();
    }
}