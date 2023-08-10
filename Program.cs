
using Authenticate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ApplicationDbContext>(config =>
{
    config.UseInMemoryDatabase("Memory");

});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
   .AddCookie()
   .AddGoogle(options =>
   {
       options.ClientId = "173793821779-ghdqv90lu6s0tqtockqn8fkru8hpae31.apps.googleusercontent.com";
       options.ClientSecret = "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";
   });

builder.Services.AddIdentity<IdentityUser, IdentityRole>(config =>
{
    config.Password.RequiredLength = 5;
    config.Password.RequireDigit = false;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "Auth_Tutorial";
    config.LoginPath = "/Home/Login";

});


builder.Services.AddMvc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();


app.UseEndpoints(endpoint =>
{
    endpoint.MapDefaultControllerRoute();

});

app.Run();