using Microsoft.AspNetCore.Authentication.Cookies;
using PetProject.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder();
var configuration = builder.Configuration;

builder.Logging.AddConsole();

builder.Services.AddMyAppServices(configuration);
builder.Services.AddSteamAppServices(configuration);

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Error/Error404";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

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

app.UseHttpsRedirection();
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Store}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
