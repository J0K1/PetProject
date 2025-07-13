using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using PetProject;
using PetProject.Extensions;
using Scalar.AspNetCore;
using StackExchange.Redis;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args); 

var configuration = builder.Configuration;

var services = builder.Services;

builder.Logging.AddConsole();

services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));

services.AddMyAppServices(configuration);
services.AddSteamAppServices(configuration);

services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Error/Error404";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true; 
    });

services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

services.AddDistributedMemoryCache();
services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
});

services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"))
    .SetApplicationName("PetProject");

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();
//    db.Database.Migrate();
//}

var supportedCultures = new[] { new CultureInfo("ru-RU") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ru-RU"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

// app.UseHttpsRedirection();

app.UseStatusCodePagesWithReExecute("/Error/{0}");

// Маршруты
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Store}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
