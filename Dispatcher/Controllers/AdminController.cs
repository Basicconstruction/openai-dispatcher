using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Dispatcher.Controllers;

[Authorize]
[Route("admin/[controller]")]
public class AdminController: Controller
{
    public IActionResult Index()
    {
        return new RedirectToPageResult("/AdminPage");
    }


}