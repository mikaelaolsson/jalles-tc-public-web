using Jalles.Core.Extensions;
using Jalles.Core.MappingProfiles.Pages;
using Jalles.Web.Extensions;
using Microsoft.IdentityModel.Logging;
using RobotsTxt;

var appEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.Sources.Clear();
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{appEnvironment}.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true)
    .Build();

builder.ConfigureKestrelLimits();

IdentityModelEventSource.ShowPII = builder.Environment.IsDevelopment();

var umbracoBuilder = builder.CreateUmbracoBuilder();
// TODO v17: kolla att detta funkar för både YouTube och Vimeo
umbracoBuilder.EmbedProviders();

umbracoBuilder
    .AddBackOffice()
    .AddAsciiFoldingToExternalIndex()
    .AddWebsite()
    .AddComposers()
    .AddContentment(x => { x.DisableTree = false; x.DisableTelemetry = true; })
    .AddAzureBlobMediaFileSystem()
    .AddAzureBlobImageSharpCache()
    .Build();

builder.Services.AddViteWithDefaults();
builder.Services.AddRobotsTxt();
builder.Services.AddHsts(options => options.MaxAge = TimeSpan.FromDays(183));
builder.Services.AddDefaultResponseCompression();

// Services
builder.Services.AddCoreServices();
builder.Services.AddUmbracoServices();

// Mappings
builder.Services.AddAutoMapper(typeof(BasePageProfile).Assembly);

var app = builder.Build();
await app.BootUmbracoAsync();

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseTrustedForwardedHeaders();
    app.UseHsts();
    app.UseRewriteRules();
}

if(!app.Environment.IsProduction())
{
    app.NoIndexOrFollow();
}

app.UseResponseCompression();
app.UseRobotsTxt();

app.UseSecurityHeaders();
app.UseAlwaysOnKeepAlive();
app.UseStaticAssetCacheControl();

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
