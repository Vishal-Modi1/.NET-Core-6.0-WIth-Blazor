using Configuration;
using FSM.Blazor.Data;
using FSM.Blazor.Data.Common;
using FSM.Blazor.Data.Company;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

var builder = WebApplication.CreateBuilder(args);
var _configurationSettings = ConfigurationSettings.Instance;


// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Backend Services
builder.Services.AddSingleton<WeatherForecastService>();

// Blazor radzen service dependencies
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddControllers();

builder.Services.AddHttpClient<HttpCaller>("FSMAPI", c =>
{
    c.BaseAddress = new Uri(ConfigurationSettings.Instance.APIURL);
});

builder.Services.AddScoped<HttpClient>();

// Add this new line
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = "myauth";
            options.Cookie.SameSite = SameSiteMode.Strict;
            // Add this new line
            options.EventsType = typeof(CookieAuthenticationEvents);    // <---
        });

// Add this new line

builder.Services.AddScoped<CookieAuthenticationEvents>();

// Add this new line

builder.Services.AddMemoryCache();

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

CurrentUserPermissionManager.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
//HttpCaller.Configure(app.Services.GetRequiredService<AuthenticationStateProvider>()); 

app.Run();
