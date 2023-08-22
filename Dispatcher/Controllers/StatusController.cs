using Dispatcher.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dispatcher.Controllers;

[ApiController]
[Route("admin/[controller]")]
public class StatusController
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