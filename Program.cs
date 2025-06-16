using PetProject.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder();
var configuration = builder.Configuration;

builder.Logging.AddConsole();

builder.Services.AddAppServices(builder.Configuration);

builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

// (по желанию) CORS, статические файлы и т.п.
builder.Services.AddCors(o => o.AddPolicy("AllowAll",
    p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

//GamesController? gamesController = app.Services.GetService<GamesController>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pages}/{action=Store}/{id?}");

app.UseStatusCodePages(async context =>
{
    await context.HttpContext.Response.WriteAsync($"{context.HttpContext.Request.Path} isn`t found {context.HttpContext.Response.StatusCode}");
});

//app.Map("/", (IConfiguration appConfig) => $"JAVA_HOME: {appConfig["JAVA_HOME"] ?? "not set"}");

//app.Use(async (context, next) =>
//{
//    if (context.Request.Path == "/")
//    {
//        context.Response.Redirect("/scalar/");
//        return;
//    }
//    await next();
//});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();