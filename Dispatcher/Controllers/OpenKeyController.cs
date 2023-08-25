using Dispatcher.Models;
using Dispatcher.Models.Entities;
using Dispatcher.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Dispatcher.Controllers;

[Authorize]
[AutoValidateAntiforgeryToken]
[Route("admin/[controller]")]
public class OpenKeyController: Controller
{
    private readonly IOpenKeyRepository _repository;
    private readonly DataContext _context;
    private IEnumerable<KeyUser> KeyUsers => _context.Users;
    public OpenKeyController(IOpenKeyRepository repository,DataContext context)
    {
        _context = context;
        _repository = repository;
    }

    [HttpGet("index")]
    public async Task<IActionResult> Index()
    {
        var openKeys = await _context.OpenKeys.Include(k => k.KeyUser)
            .ToListAsync();
        foreach (var openKey in openKeys)
        {
            if(openKey.KeyUser != null)
            {
                openKey.KeyUser.OpenKeys = null;
            }
        }
        return View(openKeys);
    }

    [HttpGet("delete")]
    public async Task<IActionResult> Delete(long id)
    {
        var open = await _repository.OpenKeys.Where(p => p.OpenKeyId == id).FirstOrDefaultAsync();
        if (open == null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View("Editor", ViewModelFactory.Delete(open,KeyUsers));
    }
    [HttpPost("delete")]
    public IActionResult Delete(OpenKey openKey)
    {
        _repository.DeleteOpenKey(openKey);
        return RedirectToAction(nameof(Index));
    }
    [HttpGet("edit")]
    public IActionResult Edit(long id)
    {
        var open = _repository.OpenKeys.Where(p => p.OpenKeyId == id).ToList().FirstOrDefault();
        return View("Editor",ViewModelFactory.Edit(open,KeyUsers));
    }
    [HttpPost("Edit")]
    public IActionResult Update([FromForm] OpenKey openKey)
    {
        if (ModelState.IsValid)
        {
            _repository.Patch(openKey);
            _repository.SaveOpenKey(openKey);
            return RedirectToAction(nameof(Index));
        }

        return View("Editor",ViewModelFactory.Edit(openKey,KeyUsers));
    }

    [HttpGet("Details")]
    public async Task<IActionResult> Details(long id)
    {
        OpenKey openKey = await _context.OpenKeys.Include(p => p.KeyUser)
            .FirstOrDefaultAsync(o => o.OpenKeyId == id);

        OpenKeyViewModel model = ViewModelFactory.Details(openKey,KeyUsers);
        return View("editor", model);
    }



    [HttpGet("create")]
    public IActionResult Create()
    {
        return View("Editor",ViewModelFactory.Create(new OpenKey(),KeyUsers));
    }

    [HttpPost("create")]
    public IActionResult Create([FromForm] OpenKey openKey)
    {
        if (ModelState.IsValid)
        {
            _repository.Patch(openKey);
            _repository.CreateOpenKey(openKey);
            return RedirectToAction(nameof(Index));
        }

        return View("Editor",ViewModelFactory.Create(openKey,KeyUsers));

    }
}

public class TrueOrFalseChoose
{
    public bool Right
    {
        get;
        set;
    }

    public string Label
    {
        get;
        set;
    }

    public static readonly List<TrueOrFalseChoose> Models = new List<TrueOrFalseChoose>()
    {
        new()
        {
            Right = false,
            Label = "False"
        },
        new ()
        {
            Right = true,
            Label = "True"
        }
    };
}
public class PricingMethodModel
{
    public PricingMethod Method
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    }

    public int Value
    {
        get;
        set;
    }
    public static readonly List<PricingMethodModel> Models = new()
    {
        new PricingMethodModel()
        {
            Method = PricingMethod.RequestTime,
            Name = "按次数",
            Value = (int)PricingMethod.RequestTime
        },
        new PricingMethodModel()
        {
            Method = PricingMethod.Token,
            Name = "按Token数",
            Value = (int)PricingMethod.Token
        },
    } ;
}