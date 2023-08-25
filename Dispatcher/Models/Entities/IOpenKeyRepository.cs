namespace Dispatcher.Models.Entities;

public interface IOpenKeyRepository
{
    IQueryable<OpenKey> OpenKeys
    {
        get;
    }

    void Patch(OpenKey openKey);
    void SaveOpenKey(OpenKey openKey);
    void CreateOpenKey(OpenKey openKey);
    void DeleteOpenKey(OpenKey openKey);
}