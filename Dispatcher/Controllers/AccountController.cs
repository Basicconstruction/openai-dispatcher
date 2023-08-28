using System.ComponentModel.DataAnnotations;
using Dispatcher.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dispatcher.Controllers;

// [Authorize]
[Route("[controller]")]
public class AccountController: Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View("index",User?.Identity?.Name);
    }

    [HttpGet("Login")]
    public ViewResult Login(string? returnUrl)
    {
        return View(new LoginModel()
        {
            ReturnUrl = returnUrl
        });
    }
    [Authorize]
    [HttpGet("Change")]
    public ActionResult ChangePassword()
    {
        return View("Change");
    }

    [HttpPost("change")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToPage("/AdminPage");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                ModelState.AddModelError("", "User not found");
            }
        }

        return View("Change", model);
    }

    [HttpPost("Login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Name);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                var success = (await _signInManager.PasswordSignInAsync
                        (user, loginModel.Password, false, false)).Succeeded;
                if (success)
                {
                    return Redirect(loginModel?.ReturnUrl ?? "/account/login");
                }
            }
        }
        ModelState.AddModelError("","Invalid name or password");
        return View(loginModel);
    }

    [Authorize]
    [HttpGet("Logout")]
    public async Task<RedirectResult> Logout(string? returnUrl)
    {
        await _signInManager.SignOutAsync();
        return Redirect(returnUrl??"/");
    }
}
public class ChangePasswordModel
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Old password is required")]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = "New password is required")]
    public string NewPassword { get; set; }
}
