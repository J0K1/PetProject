using PetProject.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder();
var configuration = builder.Configuration;

builder.Logging.AddConsole();

builder.Services.AddAppServices(builder.Configuration);

builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

builder.Services.AddControllersWithViews();

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
//app.UseCors("AllowAll");
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pages}/{action=Store}/{id?}");

app.UseHttpsRedirection();
app.MapControllers();

app.Run();