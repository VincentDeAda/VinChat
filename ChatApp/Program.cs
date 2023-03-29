using ChatApp;
using ChatApp.Models;
using ChatApp.Services;
using ChatApp.Utility;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthentication("cookie").AddCookie("cookie", x =>
{
    x.Cookie.Name = "secret";
    x.Events.OnRedirectToLogin = (c) =>
    {
        c.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});
builder.Services.AddSignalR();
builder.Services.AddControllers();
if (builder.Environment.IsDevelopment())
    builder.Services.AddSingleton<IEmailingService, EmailServiceMock>();
else 
    builder.Services.AddSingleton<IEmailingService, EmailingService>();
builder.Services.AddSingleton<IPasswordHasher<Identity>, BcryptPasswordHasher>();
builder.Services.AddDbContext<ChatContext>();
builder.Services.AddSpaStaticFiles(x =>
{
    x.RootPath = "ClientApp/dist";
});
builder.Services.AddAuthorization();
var app = builder.Build();
app.MapHub<ChatHub>("/chat");
if (!app.Environment.IsDevelopment())
{


    app.UseHsts();
    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseSpaStaticFiles();
    app.UseRouting();
    app.MapControllers();
    app.UseAuthentication();
    app.UseAuthorization();
}
else
{
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(x => x.MapControllers());
    app.UseSpa(x =>
    {
        x.UseProxyToSpaDevelopmentServer("http://localhost:5173/");
    });
}


app.Run();

