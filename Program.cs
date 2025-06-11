using Microsoft.AspNetCore.Builder;
using PetProject.Controllers;
using PetProject.Services;
using System.Diagnostics;
using System.Text.Json;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<GameService>();
builder.Services.AddSingleton<GamesController>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/scalar/");
        return;
    }
    await next();
});

//app.Map("/games/getall", async appBuilder =>
//{
//    GamesController? gamesController = app.Services.GetService<GamesController>();
//    appBuilder.Run(gamesController.HandleGetAllGames);
//});

app.UseHttpsRedirection();
app.MapControllers();

//app.Run(async (context) => await context.Response.WriteAsync($"{app.Environment.ApplicationName}"));

app.Run();
