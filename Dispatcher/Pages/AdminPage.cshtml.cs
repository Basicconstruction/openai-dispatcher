using Dispatcher.Models;
using Dispatcher.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Dispatcher.Pages;

[Authorize]
public class AdminPage : PageModel
{
    public DynamicTable Table
    {
        get;
        set;
    }

    public ServerBaseLimit ServerBaseLimit
    {
        get;
        set;
    }

    public KeyPoolRepository Repository
    {
        get;
        set;
    }
    public AdminPage(DataContext context,DynamicTable table,IOptions<ServerBaseLimit> options,KeyPoolRepository repository)
    {
        Table = table;
        ServerBaseLimit = options.Value;
        Repository = repository;
    }
    public void OnGet()
    {

    }
}