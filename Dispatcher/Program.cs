using Dispatcher.Boot;
using Dispatcher.Endpoints;
using Dispatcher.FakeGpt;
using Dispatcher.Filters;
using Dispatcher.Middlewares.api;
using Dispatcher.Models;
using Dispatcher.Models.Entities;
using Dispatcher.Models.Entities.Impl;
using Dispatcher.Models.Requests;
using Dispatcher.PreMiddleware;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => { options.Cookie.IsEssential = true; });
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.Configure<ServerBaseLimit>(opts =>
{
    opts.ServerServeLimit = 40;
    opts.IpRequestLimit = 40;
    opts.KeyRequestLimit = 40;
});
builder.Services.Configure<RunConfiguration>(opts =>
{
    opts.OpenForPublic = true;
});
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Dispatcher",
        Version = "v1"
    });
});
builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DispatcherConnection"));
});
builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString(
        "CacheConnection");
    options.SchemaName = "dbo";
    options.TableName = "DataCache";
});
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
builder.Services.Configure<CookieAuthenticationOptions>(
    IdentityConstants.ApplicationScheme
    , opts =>
    {
        opts.LoginPath = "/Account/login";
        opts.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")))
    ;

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddSingleton<KeyPoolRepository>();
builder.Services.AddSingleton<DynamicTable>();
builder.Services.AddScoped<IOpenKeyRepository, EFOpenKeyRepository>();
builder.Services.AddScoped<IPoolKeyRepository, EFPoolKeyRepository>();
builder.Services.AddSingleton<Syncer>();
builder.Services.AddSingleton<Starter>();
builder.Services.Configure<MvcOptions>(opts =>
{
    opts.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(value=>"please enter a value");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/dashboard",new DashboardOptions{
    Authorization = new [] { new HangfireAuthorizationFilter() }});


app.Map("/v1", apiApp =>
{
    //apiApp.UseMiddleware<TestMiddleware>();

    apiApp.UseMiddleware<RosterMiddleware>();
    apiApp.UseMiddleware<SecureMiddleware>();
    apiApp.UseMiddleware<ModelFilterMiddleware>();
    apiApp.UseMiddleware<RequestChooserMiddleware>();
    apiApp.UseMiddleware<PricingMiddleware>();
    apiApp.UseEndpoints(endpoints =>
        { endpoints.MapPost("v1/{*path}", new TransferEndpoint().Endpoint); });
});
app.Map("/test/v1", apiApp =>
{
    apiApp.UseMiddleware<SecureMiddleware>();
    apiApp.UseMiddleware<PricingMiddleware>();
    apiApp.UseEndpoints(endpoints => { endpoints.MapPost("test/v1/{*path}", new TestTransferEndpoint().Endpoint); });
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
    endpoints.MapHangfireDashboard();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseSwagger();
app.UseSwaggerUI(opts => { opts.SwaggerEndpoint("/swagger/v1/swagger.json", "ws"); });
AutoMigration.Migration(app);
BackgroundJob.Enqueue<Starter>(starter => starter.Init());
RecurringJob.AddOrUpdate<DynamicTable>("easyJob",
    table => table.Reset(), Cron.Minutely);
RecurringJob.AddOrUpdate<Syncer>("fetchKey",syncer=> syncer.UpdateDynamicKeys(),Cron.Minutely);
await SeedIdentityUser.Ensure(app);
if (!app.Environment.IsDevelopment())
{
    RecurringJob.TriggerJob("fetchKey");
}
app.Run();

//{
//  "profiles": {
//    "Dispatcher": {
//      "commandName": "Project",
//      "launchBrowser": true,
//      "environmentVariables": {
//        "ASPNETCORE_ENVIRONMENT": "Development"
//      },
//      "dotnetRunMessages": true,
//      "applicationUrl": "https://localhost:7175;http://localhost:5191"
//    },
//    "IIS Express": {
//      "commandName": "IISExpress",
//      "launchBrowser": true,
//      "environmentVariables": {
//        "ASPNETCORE_ENVIRONMENT": "Development"
//      }
//    },
//    "Docker": {
//      "commandName": "Docker",
//      "launchBrowser": true,
//      "launchUrl": "{Scheme}://{ServiceHost}:{ServicePort}",
//      "publishAllPorts": true,
//      "useSSL": true
//    }
//  },
//  "iisSettings": {
//    "windowsAuthentication": false,
//    "anonymousAuthentication": true,
//    "iisExpress": {
//      "applicationUrl": "http://localhost:17460",
//      "sslPort": 44343
//    }
//  }
//}






//
// {
//     "profiles": {
//         "Dispatcher": {
//             "commandName": "Project",
//             "launchBrowser": true,
//             "environmentVariables": {
//                 "ASPNETCORE_ENVIRONMENT": "Production"
//             },
//             "dotnetRunMessages": true,
//             "applicationUrl": "https://localhost:7175;http://localhost:5191"
//         },
//         "IIS Express": {
//             "commandName": "IISExpress",
//             "launchBrowser": true,
//             "environmentVariables": {
//                 "ASPNETCORE_ENVIRONMENT": "Production"
//             }
//         },
//         "Docker": {
//             "commandName": "Docker",
//             "launchBrowser": true,
//             "launchUrl": "{Scheme}://{ServiceHost}:{ServicePort}",
//             "publishAllPorts": true,
//             "useSSL": true
//         }
//     },
//     "iisSettings": {
//         "windowsAuthentication": false,
//         "anonymousAuthentication": true,
//         "iisExpress": {
//             "applicationUrl": "http://localhost:17460",
//             "sslPort": 44343
//         }
//     }
// }