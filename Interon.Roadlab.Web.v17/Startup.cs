using Interon.Roadlab.Web.Net.Core.Config;
using Interon.Roadlab.Web.Net.Core.Controllers;
using Interon.Roadlab.Web.Net.Core.Middleware;
using Interon.Roadlab.Web.Net.Core.Services;
using Our.Umbraco.FullTextSearch;

namespace Interon.Roadlab.Web.v17
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="webHostEnvironment">The web hosting environment.</param>
        /// <param name="config">The configuration.</param>
        /// <remarks>
        /// Only a few services are possible to be injected here https://github.com/dotnet/aspnetcore/issues/9337.
        /// </remarks>
        public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            _env = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _config = config ?? throw new ArgumentNullException(nameof(config));

        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940.
        /// </remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<EmailSettings>(_config.GetSection("EmailSettings"));
	        services.AddHttpContextAccessor();
            
            // Register spam filter service
            services.AddScoped<ISpamFilterService, SpamFilterService>();
            
            services.AddUmbraco(_env, _config)
                .AddBackOffice()
                .AddWebsite()

                .AddComposers()
                .Build();

            // Razor runtime compilation: required while RazorCompileOnBuild=false
            // is in effect during the v17 view-migration window.
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The web hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMiddleware<MediaFileMiddleware>();
            }
            else
            {
                // HSTS only fires in non-development environments (UseHsts respects this anyway,
                // but being explicit prevents an accidental HSTS lock on localhost).
                app.UseHsts();
            }

            // Defence-in-depth response headers (nosniff, SAMEORIGIN, Referrer-Policy).
            // CSP is intentionally deferred — to be introduced in report-only mode first.
            app.UseMiddleware<SecurityHeadersMiddleware>();

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
        }
    }
}
