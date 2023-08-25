using Dispatcher.Models;
using Dispatcher.Models.Requests;

namespace Dispatcher.FakeGpt;

public class Syncer
{
    private DataContext _context;
    private KeyPoolRepository _repository;

    public Syncer(DataContext context,KeyPoolRepository repository)
    {
        _context = context;
        _repository = repository;
    }
    public void UpdateDynamicKeys()
    {
        _repository.PoolKeys = _context.PoolKeys.Where(p=>p.Available==true).ToList();
    }
}