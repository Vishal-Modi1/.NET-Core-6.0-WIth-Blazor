using AspNetCoreRateLimit;
using Configuration;
using DataModels.Constants;
using FSMAPI.CustomServicesExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var _configurationSettings = ConfigurationSettings.Instance;
var _environment = builder.Environment;

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                            },
                        new string[] {}
                    }
                });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

#region API rate limit configuraiton

builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.HttpStatusCode = 429;
    options.RealIpHeader = "X-Real-IP";
    options.ClientIdHeader = "X-ClientId";
    options.GeneralRules = new List<RateLimitRule>
        {
            new RateLimitRule
            {
                Endpoint = "POST:/api/company/create",
                Period = "10s",
                Limit = 2,
            },
             new RateLimitRule
            {
                Endpoint = "POST:/api/user/create",
                Period = "10s",
                Limit = 2,
            },
             new RateLimitRule
            {
                Endpoint = "PUT:/api/user/updatecreatedby",
                Period = "10s",
                Limit = 2,
            }
        };
});

builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();

#endregion

#region

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = true,
        ValidIssuer = _configurationSettings.JWTIssuer,
      //  ValidAudience = _configurationSettings.JWTIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationSettings.JWTKey)),
        ClockSkew = TimeSpan.Zero // remove delay of token when expire
    };
});

builder.Services.AddAuthorization(cfg =>
{
    cfg.AddPolicy("ClearanceLevel1", policy => policy.RequireClaim("ClearanceLevel", "1", "2"));
    cfg.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    cfg.AddPolicy("OfficeStaff", policy => policy.RequireClaim(ClaimTypes.Role, "OfficeStaff"));
    cfg.AddPolicy("Instructors", policy => policy.RequireClaim(ClaimTypes.Role, "Instructors"));
    cfg.AddPolicy("Rentors", policy => policy.RequireClaim(ClaimTypes.Role, "Rentors"));
    cfg.AddPolicy("Students", policy => policy.RequireClaim(ClaimTypes.Role, "Students"));
    cfg.AddPolicy("ReadOnly", policy => policy.RequireClaim(ClaimTypes.Role, "ReadOnly"));

});

#endregion

builder.Services.AddHttpContextAccessor();

//Services
builder.Services.AddCustomServices();

//Repositories
builder.Services.AddCustomRepositories();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (_environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

string uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), UploadDirectory.RootDirectory);
Directory.CreateDirectory(uploadsPath);

Directory.CreateDirectory(uploadsPath + "\\" + UploadDirectory.AircraftImage);
Directory.CreateDirectory(uploadsPath + "\\" + UploadDirectory.UserProfileImage);
Directory.CreateDirectory(uploadsPath + "\\" + UploadDirectory.Document);
Directory.CreateDirectory(uploadsPath + "\\" + UploadDirectory.CompanyLogo);

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
    Path.Combine(Directory.GetCurrentDirectory(), UploadDirectory.RootDirectory)),
    RequestPath = $"/{UploadDirectory.RootDirectory}"
});

//Enable directory browsing
app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), UploadDirectory.RootDirectory)),
    RequestPath = $"/{UploadDirectory.RootDirectory}"
});


app.UseHttpsRedirection();

app.UseRouting();

app.UseIpRateLimiting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHttpsRedirection();


app.MapControllers();

app.Run();
