using Configuration;
using DataModels.Constants;
using FSMAPI.CustomServicesExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
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
});

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
        ValidIssuer = _configurationSettings.JWTIssuer,
        ValidAudience = _configurationSettings.JWTIssuer,
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
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
}

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

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHttpsRedirection();


app.MapControllers();

app.Run();
