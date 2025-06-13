using PetProject.Controllers;
using PetProject.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<GameService>();
builder.Services.AddSingleton<GamesController>();

builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<UserController>();

builder.Logging.AddConsole();

var app = builder.Build();

//GamesController? gamesController = app.Services.GetService<GamesController>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseStatusCodePages(async context =>
{
    await context.HttpContext.Response.WriteAsync($"{context.HttpContext.Request.Path} isn`t found {context.HttpContext.Response.StatusCode}");
});

//app.Map("/", (IConfiguration appConfig) => $"JAVA_HOME: {appConfig["JAVA_HOME"] ?? "not set"}");

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/scalar/");
        return;
    }
    else if (context.Request.Path == "/g")
    {
        context.Response.Redirect("/games/1");
        return;
    }
    await next();
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();