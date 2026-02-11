using Jalles.Core.Contracts;
using Jalles.Core.Extensions;
using Jalles.Core.MappingProfiles.Pages;
using Jalles.Core.Services;
using Jalles.Web.Extensions;
using Jalles.Web.Services;
using Microsoft.AspNetCore.ResponseCompression;
using RobotsTxt;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.IdentityModel.Logging;
using Umbraco.Cms.Core.Media.EmbedProviders;

namespace Jalles.Web;

public class Startup
{
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _configuration;

    public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration config)
    {
        _env = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        _configuration = config ?? throw new ArgumentNullException(nameof(config));
    }

    public void ConfigureServices(IServiceCollection services)
    {
        IdentityModelEventSource.ShowPII = _env.IsDevelopment();

        var umbraco = services.AddUmbraco(_env, _configuration);

        umbraco.EmbedProviders()
            .Replace<YouTube, YoutubeExtensions>()
            .Replace<Vimeo, VimeoExtensions>();

        umbraco
            .AddBackOffice()
            .AddWebsite()
            //.AddDeliveryApi()
            .AddComposers()
            .AddAzureBlobMediaFileSystem()
            .Build();

        services.AddHsts(options => options.MaxAge = TimeSpan.FromDays(183));

        services.AddResponseCompression(options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
            options.EnableForHttps = true;
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
        });

        // Services
        services.AddScoped<IContentAccessor, ContentAccessor>();
        services.AddScoped<IMixedListingBlockService, MixedListingBlockService>();
        services.AddScoped<IPaginationService, PaginationService>();
        services.AddScoped<IFilterService, FilterService>();

        // Mappings
        services.AddAutoMapper(typeof(BasePageProfile).Assembly);

        services.AddRobotsTxt();

        services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if(env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/error");
            app.UseDeveloperExceptionPage();
            app.UseHsts();
            app.UseRewriter(new RewriteOptions()
                .Add(new RedirectPublicDomainsToWww())
                .Add(new RedirectFromAzureWebsites())
            );
        }

        app.UseResponseCompression();

        app.Use(async (context, next) =>
        {
            var requestPath = context.Request.Path;

            context.Response.Headers.Append("X-Xss-Protection", "1; mode=block");
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Frame-Options", "SAMEORIGIN");
            context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
            context.Response.Headers.Append("Feature-Policy",
                "geolocation 'none'; midi 'none'; sync-xhr 'none'; microphone 'none'; camera 'none'; magnetometer 'none'; gyroscope 'none'; fullscreen *; payment 'none';");

            if(!requestPath.StartsWithSegments("/umbraco") && !requestPath.StartsWithSegments("/App_Plugins"))
            {
                context.Response.Headers.Append("Content-Security-Policy",
                    "default-src data: blob: filesystem: about: ws: wss: frame-src: * 'unsafe-inline' 'unsafe-eval'; media-src *; script-src * data: blob: 'unsafe-inline'; connect-src * data: blob: 'unsafe-inline'; img-src * data: blob: 'unsafe-inline'; style-src * data: blob: 'unsafe-inline';font-src * data: blob: 'unsafe-inline'; frame-ancestors * data: blob:; object-src 'none'; form-action 'self'");
            }

            await next();
        });

        app.Use(async (context, next) =>
        {
            var userAgent = context.Request.Headers.UserAgent.FirstOrDefault();

            if(userAgent == "AlwaysOn")
            {
                context.Request.Path = "/keep-alive";
            }

            await next();
        });

        app.Use(async (context, next) =>
        {
            var path = context.Request.Path.Value;

            if(path?.StartsWith("/umbraco/") != false)
            {
                await next();
                return;
            }

            var cachableExtensions = new[] { ".js", ".css", ".woff", ".woff2", ".svgz", ".svg" };
            if(cachableExtensions.Any(extension => path.EndsWith(extension)) || path.StartsWith("/media/"))
            {
                context.Response.Headers.Append("Cache-Control", "public, max-age=31536000");
            }

            await next();
        });

        // Static robots.txt middleware
        app.UseRobotsTxt();

        if(!env.IsProduction())
        {
            app.NoIndexOrFollow(env);
        }

        app.UseUmbraco()
            .WithMiddleware(u =>
            {
                u.UseBackOffice();
                u.UseWebsite();
            })
            .WithEndpoints(u =>
            {
                u.UseInstallerEndpoints();
                u.UseBackOfficeEndpoints();
                u.UseWebsiteEndpoints();
            });
    }
}
