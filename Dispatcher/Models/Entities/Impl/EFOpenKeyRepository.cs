namespace Dispatcher.Models.Entities.Impl;

public class EFOpenKeyRepository: IOpenKeyRepository
{
    private DataContext _context;

    public EFOpenKeyRepository(DataContext context)
    {
        _context = context;
    }

    public IQueryable<OpenKey> OpenKeys => _context.OpenKeys;
    public void Patch(OpenKey openKey)
    {
        openKey.KeyUser = _context.Users.FirstOrDefault(k => k.KeyUserId == openKey.KeyUserId);
    }

    public void SaveOpenKey(OpenKey openKey)
    {
       // var got = _context.OpenKeys.Find(openKey.OpenKeyId);
        var got = _context.OpenKeys.Where(p=>p.OpenKeyId == openKey.OpenKeyId).FirstOrDefault();
        if (got != null)
        {
            got.CopyFrom(openKey);
        }
        _context.SaveChanges();
    }

    public void CreateOpenKey(OpenKey openKey)
    {
        if (openKey.KeyUser != null)
        {
            _context.Attach(openKey.KeyUser);
        }
        if (openKey.OpenKeyId == 0)
        {
            _context.Add(openKey);
        }

        _context.SaveChanges();
    }

    public void DeleteOpenKey(OpenKey openKey)
    {
        _context.Remove(openKey);
        _context.SaveChanges();
    }
}