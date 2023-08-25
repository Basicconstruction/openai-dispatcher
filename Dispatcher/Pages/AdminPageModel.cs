using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dispatcher.Pages;

[Authorize(Roles = "Admins")]
public class AdminPageModel: PageModel
{
    
}