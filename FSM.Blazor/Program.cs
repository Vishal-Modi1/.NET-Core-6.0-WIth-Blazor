using Configuration;
using DataModels.Constants;
using FSM.Blazor.CustomServicesExtensions;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using Radzen;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);
var _configurationSettings = ConfigurationSettings.Instance;


// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSyncfusionBlazor();

// Custom Backend Services
builder.AddCustomServices();

// Blazor radzen service dependencies
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddServerSideBlazor().AddHubOptions(o =>
{
    o.MaximumReceiveMessageSize = 10 * 1024 * 1024;
});

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
  //  app.UseExceptionHandler("/Error");
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseExceptionHandler(
                options =>
                {
                    options.Run(
                        async context =>
                        {
                            
                        });
                }
            );

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(ConfigurationSettings.Instance.SyncFusionLicenseKey);

string uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), UploadDirectory.RootDirectory);
Directory.CreateDirectory(uploadsPath);

Directory.CreateDirectory(uploadsPath + "\\" + UploadDirectory.TempDocument);

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

//CurrentUserPermissionManager.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
//HttpCaller.Configure(app.Services.GetRequiredService<AuthenticationStateProvider>()); 

app.Run();
