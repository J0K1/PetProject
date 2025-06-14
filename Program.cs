using Microsoft.EntityFrameworkCore;
using PetProject;
using PetProject.Controllers;
using PetProject.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder();
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<GamesController>();

builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<UserController>();

builder.Services.AddDbContext<AppDBContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(AppDBContext)));
    });

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