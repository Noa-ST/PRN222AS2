using Microsoft.EntityFrameworkCore;
using FUNewsManagementSystem.Core.Data;
using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Repositories;
using FUNewsManagementSystem.Core.Services;
using FUNewsManagementSystem.Hubs;
using FUNewsManagementSystem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/Login", "");
})
#if DEBUG
    .AddRazorRuntimeCompilation();
#endif

// SignalR
builder.Services.AddSignalR();

// DbContext
builder.Services.AddDbContext<FUNewsManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FUNewsDb")));

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Repositories
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<INewsArticleTagRepository, NewsArticleTagRepository>();

// Services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<INewsArticleTagService, NewsArticleTagService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login";
        options.ReturnUrlParameter = "returnUrl";
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.IsEssential = true;
        options.Events = new CookieAuthenticationEvents
        {
            OnSignedIn = context =>
            {
                Console.WriteLine("User signed in. Cookie issued.");
                return Task.CompletedTask;
            },
            OnRedirectToLogin = context =>
            {
                Console.WriteLine($"Redirecting to Login. OriginalPath: {context.Request.Path}");
                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                Console.WriteLine($"Redirecting to AccessDenied. OriginalPath: {context.Request.Path}");
                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
Console.WriteLine("Authentication middleware applied");

app.UseAuthorization();
app.UseSession();
Console.WriteLine("Session middleware applied");
app.MapHub<NewsHub>("/newshub");
app.MapRazorPages();
Console.WriteLine("RazorPages middleware applied");

app.Run();
