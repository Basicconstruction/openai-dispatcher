using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dispatcher.Models;

public class SeedIdentityUser
{
    private const string AdminUser = "admin";
    public static async Task Ensure(IApplicationBuilder app)
    {
        await using var context = app.ApplicationServices.CreateScope().ServiceProvider
            .GetRequiredService<AppIdentityDbContext>();
        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
        }

        using var userManager = app.ApplicationServices.CreateScope().ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>();
        var user = await userManager.FindByIdAsync(AdminUser);
        if (user == null)
        {
            user = new IdentityUser(AdminUser)
            {
                Email = "ai-included.com",
                PhoneNumber = "3849023"
            };
            await userManager.CreateAsync(user, "Sk-4399");
        }

    }
}