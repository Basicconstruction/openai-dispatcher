using Dispatcher.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dispatcher.Controllers;

[ApiController]
[Authorize]
[Route("admin/[controller]")]
public class StatusController: Controller
{
    private DynamicTable _table;

    public StatusController(DynamicTable table)
    {
        _table = table;
    }

    [HttpGet]
    public DynamicTable Table()
    {
        return _table;
    }
}