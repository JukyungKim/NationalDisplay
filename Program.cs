using NationalDisplay.Models;
using Microsoft.AspNetCore.ResponseCompression;
using NationalDisplay.Controllers;

Console.WriteLine("National Display starts");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var app = builder.Build();

PipeClient.Start();
SensorTask.Start();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// var webSocketOptions = new WebSocketOptions
// {
//     KeepAliveInterval = TimeSpan.FromMilliseconds(5000)
// };
// app.UseWebSockets(webSocketOptions);

app.MapHub<SensorHub>("/sensorHub");

Console.WriteLine("app run");
app.Run();
