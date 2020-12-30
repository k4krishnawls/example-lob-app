using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CorrelationId;
using CorrelationId.DependencyInjection;
using ELA.App.Controllers.General.Utility;
using ELA.App.ErrorHandling;
using ELA.App.HealthChecks;
using ELA.App.Security;
using ELA.App.StartupConfiguration;
using ELA.Business.Authentication;
using ELA.Common.Authentication;
using ELA.Common.Persistence;
using ELA.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

namespace ELA.App
{
    public class Startup
    {
        private IWebHostEnvironment _environment;
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configurations
            services.AddScoped<DatabaseConnectionSettings>((services) => {
                return new DatabaseConnectionSettings() { Database = _configuration.GetConnectionString("Database") };
            });

            // Data
            services.AddScoped<IPersistence, DapperPersistence>();

            // Business/Domain Logic
            BusinessServiceConfiguration.Configure(services);

            // interactive security
            services.AddAntiforgery();
            services.AddAuthentication(SecurityConstants.CookieAuthScheme)
                .AddCookie(SecurityConstants.CookieAuthScheme, options =>
                {
                    options.LoginPath = "/account/login";
                    options.AccessDeniedPath = "/account/accessdenied";
                    options.LogoutPath = "/account/logout";
                });
            services.AddScoped<IAccountCookies, AccountCookies>();
            services.AddScoped<ISignInManager, SignInManager>();
            // TODO: configure Data Protection

            // Policies
            {
                // Cookie policies
                services.Configure<CookiePolicyOptions>(options =>
                {
                    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
                    options.HttpOnly = HttpOnlyPolicy.None;
                    options.Secure = CookieSecurePolicy.Always;
                });

                // Authorizations policies
                services.AddAuthorization(options =>
                {
                    options.AddPolicy(Policies.InteractiveUserAccess, builder =>
                    {
                        builder.RequireAuthenticatedUser();
                        builder.AuthenticationSchemes.Add(SecurityConstants.CookieAuthScheme);
                        builder.RequireClaim(ClaimNames.SessionId);
                        builder.RequireClaim(ClaimNames.UserId);
                        builder.RequireClaim(ClaimNames.UserName);
                    });

                    options.DefaultPolicy = options.GetPolicy(Policies.InteractiveUserAccess);
                });

                // CORS policies
                services.AddCors(options =>
                {
                    options.AddPolicy(SecurityConstants.CORS_AllowAny, builder =>
                    {
                        builder.AllowAnyOrigin();
                    });
                });
            }

            // Endpoints
            {
                services.AddDefaultCorrelationId();

                // Health
                services.AddHealthChecks()
                    .AddCheck<DatabaseHealthCheck>("database");

                // MVC 
                services.AddControllersWithViews(options => {
                    options.Filters.Add(new UnhandledApiExceptionFilter(new string[] { 
                        // add API endpoints here to automaically return non-HTML errors
                        "/api/fe"
                    }));
                });
                services.Configure<RazorViewEngineOptions>(o =>
                {
                    // {2} is area, {1} is controller,{0} is the action    
                    o.ViewLocationFormats.Clear();
                    o.ViewLocationFormats.Add("/Controllers/{1}/Views/{0}" + RazorViewEngine.ViewExtension);
                    o.AreaViewLocationFormats.Add("/Controllers/{2}/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                });

                // SPA
                services.AddSpaStaticFiles(configuration =>
                {
                    configuration.RootPath = "ClientApp/dist";
                });
            }

            // Compression
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // -- Development tasks
            if (env.IsDevelopment())
            {
                LocalDevelopmentTasks.MigrateDatabase(_configuration.GetConnectionString("Database"));
            }

            // -- Continue configuration

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
                // use HTTP locally to support HMR w/ parcel
                app.UseHttpsRedirection();
            }
            app.UseResponseCompression();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCorrelationId();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = HealthCheckResponse.WriteResponse
                })
                .RequireCors(SecurityConstants.CORS_AllowAny);

                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Client SPA only from here forwards
            {
                // I haven't figured out how to apply a security policy
                //  from the Startup, so we'll do the default challenge and
                //  that does the right thing for now
                app.Use(async (context, next) =>
                {
                    if (!context.User.Identity.IsAuthenticated)
                    {
                        if (context.Request.Path.StartsWithSegments("/backend.css"))
                        {
                            await next();
                        }
                        else
                        {
                            await context.ChallengeAsync();
                        }
                    }
                    else
                    {
                        await next();
                    }
                });

                app.UseSpaStaticFiles(new StaticFileOptions()
                {
                    OnPrepareResponse = ctx =>
                    {
                        if (ctx.Context.Request.Path.StartsWithSegments("/static"))
                        {
                            // Cache all static resources for 1 year (versioned filenames)
                            var headers = ctx.Context.Response.GetTypedHeaders();
                            headers.CacheControl = new CacheControlHeaderValue
                            {
                                Public = true,
                                MaxAge = TimeSpan.FromDays(365)
                            };
                        }
                        else
                        {
                            // Do not cache explicit `/index.html` or any other files.  See also: `DefaultPageStaticFileOptions` below for implicit "/index.html"
                            var headers = ctx.Context.Response.GetTypedHeaders();
                            headers.CacheControl = new CacheControlHeaderValue
                            {
                                Public = true,
                                MaxAge = TimeSpan.FromDays(0)
                            };
                        }
                    }
                });
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp/dist";
                    spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions()
                    {
                        OnPrepareResponse = ctx =>
                        {
                            // Do not cache implicit `/index.html`.  See also: `UseSpaStaticFiles` above
                            var headers = ctx.Context.Response.GetTypedHeaders();
                            headers.CacheControl = new CacheControlHeaderValue
                            {
                                Public = true,
                                MaxAge = TimeSpan.FromDays(0)
                            };
                        }
                    };

                    if (env.IsDevelopment())
                    {
                        spa.Options.DefaultPage = "/index.html";
                        spa.Options.StartupTimeout = TimeSpan.FromSeconds(60);
                        LocalDevelopmentTasks.StartFrontendService(spa, "../../frontend/react-parcel-ts", "yarn", (port) => $"run start --port {port}", "Server running at ");
                    }
                });
            }
        }
    }
}
