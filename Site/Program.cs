using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Site.Context;
using Site.Hubs;
using Site.Models.Services;

var builder = WebApplication.CreateBuilder(args);

var MvcBuilder = builder.Services.AddControllersWithViews();
#if DEBUG
MvcBuilder.AddRazorRuntimeCompilation();
#endif
builder.Services.AddSignalR();

string ConnectionString = "Data Source=.\\Tadarokat;Initial Catalog=SignalR_DB;Integrated Security=True;";
builder.Services.AddDbContext<DataBaseContext>(option => option.UseSqlServer(ConnectionString));

builder.Services.AddScoped<IChatRoomService, ChatRoomService>();
builder.Services.AddScoped<IMessgaeService, MessgaeService>();
builder.Services.AddAuthentication(option =>
{
    option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(Option => Option.LoginPath = "/Home/Login");

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<SiteChatHub>("/chathub");
app.MapHub<SupportHub>("/supporthub");
app.Run();
