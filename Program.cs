using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using PetProject;
using PetProject.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args); 

var configuration = builder.Configuration;

var services = builder.Services;

builder.Logging.AddConsole();

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
