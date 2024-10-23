using System;
using System.Linq;
using dotnetcoreproject.Application;
using dotnetcoreproject.Infrastructure.Data;
using dotnetcoreproject.Infrastructure.Extensions;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using UI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();


// Add services to the container
builder.Services.AddControllersWithViews(options => { })
    .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; })
    .AddRazorRuntimeCompilation();


// Register application and infrastructure services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddEntityFrameworkServices(builder.Configuration);

// Add Session and Response Caching
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddResponseCaching();


var app = builder.Build();
app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);
// Ensure database is updated and seeded
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await ApplicationDbContext.EnsureDatabaseUpdatedAndSeeded(services);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while migrating or seeding the database.");
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

builder.Services.AddHttpContextAccessor();
// Add Session middleware
app.UseSession();
SessionRequest.Configure(app.Services.GetRequiredService<IHttpContextAccessor>(), app.Configuration);

app.UseAuthentication();
app.UseAuthorization();


app.UseResponseCaching();
app.UseResponseCacheMiddleware();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        "default",
        "{controller=Home}/{action=Index}/{id?}");
});

app.Run();