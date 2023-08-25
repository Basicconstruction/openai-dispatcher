using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dispatcher.Models.ViewModels;

public class OpenKeyViewModel
{
    public OpenKey? OpenKey
    {
        get;
        set;
    }

    public string Action
    {
        get;
        set;
    } = "Create";

    public bool ReadOnly
    {
        get;
        set;
    } = false;

    public string Theme
    {
        get;
        set;
    } = "primary";

    public bool ShowAction
    {
        get;
        set;
    } = true;


    public IEnumerable<KeyUser> KeyUsers
    {
        get;
        set;
    }

    public SelectList KeyUserSelector => new SelectList(
        KeyUsers,"KeyUserId","UserName");
    public SelectList AvailableSelector => NormalSelector.TrueOrFalseSelectList;
    public SelectList PricingSelector => NormalSelector.PricingSelectList;
}