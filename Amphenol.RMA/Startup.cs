using Amphenol.RMA.AccesoDatos.Data;
using Amphenol.RMA.AccesoDatos.Data.Repository;

using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRChat.Hubs;
using System;

namespace Amphenol.RMA
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAntiforgery(options => { options.SuppressXFrameOptionsHeader = true; });

            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            services.AddControllers();

            services.AddHttpContextAccessor();

            services.AddSignalR();

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));
            services.AddHangfireServer();

            services.AddDbContext<DbContext100>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("Connection100")));
            services.AddDbContext<DbContextM10>(builder => builder
               .UseSqlServer(Configuration.GetConnectionString("ConnectionM10")));
            services.AddControllersWithViews();
            services.AddScoped<IContenedorTrabajo, ContenedorTrabajo>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) => { context.Response.Headers.Remove("X-Frame-Options"); await next(); });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                app.UseHsts();
            }


            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHangfireDashboard();
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                  name: "default",
                   pattern: "{area=Client}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapBlazorHub();

                endpoints.MapRazorPages();


                endpoints.MapHub<RmaHub>("/rmaHub");
            });
        }




    }
}

