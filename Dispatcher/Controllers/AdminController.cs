using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dispatcher.Controllers;

[Authorize]
[Microsoft.AspNetCore.Components.Route("admin/[controller]")]
public class AdminController
{
    public IActionResult Index()
    {
        return new RedirectToPageResult("/AdminPage");
    }
}