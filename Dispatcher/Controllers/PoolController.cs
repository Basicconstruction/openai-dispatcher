using Dispatcher.Models;
using Dispatcher.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dispatcher.Controllers;

[ApiController]
[Route("admin/[controller]")]
public class PoolController: Controller
{
    private readonly DataContext _context;
    private readonly KeyPoolRepository _repository;
    public PoolController(DataContext context,KeyPoolRepository repository)
    {
        _context = context;
        _repository = repository;
    }
    [HttpPost("{keys}")]
    public async Task<List<PoolKey>> GetKeysAsync()
    {
        return await _context.PoolKeys.ToListAsync();
    }

    [HttpPost("Add")]
    public IActionResult OnAddKey(PoolKey poolKey)
    {
        _context.Add(poolKey);
        _repository.Add(poolKey);
        return View("index");
    }
}