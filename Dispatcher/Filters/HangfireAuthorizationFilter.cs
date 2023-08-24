using Hangfire.Dashboard;

namespace Dispatcher.Filters;

public class HangfireAuthorizationFilter: IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return context.GetHttpContext().User.Identity?.IsAuthenticated??false;
    }
}