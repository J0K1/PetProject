var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5003, listenOptions =>
    {
        listenOptions.UseHttps("certs/devcert.pfx", "123");
    });
});

var app = builder.Build();

app.Run();
