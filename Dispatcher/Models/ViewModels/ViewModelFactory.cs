namespace Dispatcher.Models.ViewModels;

public class ViewModelFactory
{
    public static OpenKeyViewModel Details(OpenKey openKey, IEnumerable<KeyUser> keyUsers)
    {
        return new OpenKeyViewModel()
        {
            OpenKey = openKey,
            ReadOnly = true,
            Action = "Details",
            KeyUsers = keyUsers,
            ShowAction = false
        };
    }

    public static OpenKeyViewModel Create(OpenKey openKey, IEnumerable<KeyUser> keyUsers)
    {
        return new OpenKeyViewModel()
        {
            OpenKey = openKey,
            KeyUsers = keyUsers
        };
    }

    public static OpenKeyViewModel Edit(OpenKey openKey, IEnumerable<KeyUser> keyUsers)
    {
        return new OpenKeyViewModel()
        {
            OpenKey = openKey,
            Action = "Edit",
            KeyUsers = keyUsers
        };
    }

    public static OpenKeyViewModel Delete(OpenKey openKey, IEnumerable<KeyUser> keyUsers)
    {
        return new OpenKeyViewModel()
        {
            OpenKey = openKey,
            Action = "Delete",
            KeyUsers = keyUsers,
            ReadOnly = true
        };
    }
}