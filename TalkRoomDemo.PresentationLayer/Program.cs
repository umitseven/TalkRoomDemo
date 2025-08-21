using Microsoft.AspNetCore.Identity;
using TalkRoomDemo.businessLayer.Abstract;
using TalkRoomDemo.businessLayer.Concrete;
using TalkRoomDemo.DataAccessLayer.Abstract;
using TalkRoomDemo.DataAccessLayer.AppDbContext;
using TalkRoomDemo.DataAccessLayer.EntityFramwork;
using TalkRoomDemo.EntityLayer.Concrete;
using TalkRoomDemo.PresentationLayer.Hubs;
using TalkRoomDemo.PresentationLayer.Models;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using AspNetCoreHero.ToastNotification.Notyf;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Toast servisini ekle
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});




builder.Services.AddSignalR();
builder.Services.AddDbContext<Context>();
   
builder.Services.AddIdentity<AppUser,AppRole>().AddEntityFrameworkStores<Context>().AddErrorDescriber<CustomIdentityValidator>();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true; // Email benzersiz olmalý!
});
builder.Services.AddScoped<IFriendService, FriendsManager>();
builder.Services.AddScoped<IFriendsDal, EfFriendsDal>();
builder.Services.AddScoped<IServerService, ServerManager>();
builder.Services.AddScoped<IServerDal, EfSereverDal>();
builder.Services.AddScoped<IMessageService, MessageManager>();
builder.Services.AddScoped<IMessageDal, EfMessageDal>();
builder.Services.AddScoped<IFriendRequestService, FriendRequestManager>();
builder.Services.AddScoped<IFriendRequestDal, EfFriendRequestDal>();
builder.Services.AddScoped<IServerMessageService, ServerMessageManager>();
builder.Services.AddScoped<IServerMessageDal, EfServerMessageDal>();
builder.Services.AddScoped<IServerUserService, ServerUserManager>();
builder.Services.AddScoped<IServerUserDal, EfServerUserDal>();
builder.Services.AddSingleton<OnlineUserCache>(); // Online kullanýcýlarý takip etmek için singleton bir servis ekliyoruz


var app = builder.Build();

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
app.UseAuthentication();
app.UseAuthorization();
app.UseNotyf();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chatHub");
app.Run();
