using Microsoft.EntityFrameworkCore;

namespace Dispatcher.Models;

public class AutoMigration
{
    public static void Migration(IApplicationBuilder app)
    {
        var serviceProvider = app.ApplicationServices.CreateScope().ServiceProvider;
        using var context = serviceProvider.GetRequiredService<DataContext>();
        context.Database.Migrate();
    }
}