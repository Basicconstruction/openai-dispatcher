using Dispatcher.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dispatcher.Controllers;

[Authorize]
[Route("admin/[controller]")]
public class PoolKeyController: Controller
{
    private DataContext _context;

    public PoolKeyController(DataContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        var poolKeys = _context.PoolKeys;
        return View(poolKeys);
    }
}