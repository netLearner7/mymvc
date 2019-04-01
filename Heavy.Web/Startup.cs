using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Heavy.Web.Data;
using Heavy.Web.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Heavy.Web.Models;
using Heavy.Web.Auth;
using Microsoft.AspNetCore.Authorization;
using System;
using Heavy.Web.Filters;
using System.Security.Policy;

namespace Heavy.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser,IdentityRole>(options=> {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<HeavyContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                });

            services.AddScoped<IAlbumService, AlbumEfService>();

            services.AddAuthorization(option =>
            {
                option.AddPolicy("管理员策略", p => p.RequireRole("zyz2"));
                option.AddPolicy("音乐编辑", p => p.RequireClaim("edit"));

                

                option.AddPolicy("音乐编辑1", p => p.RequireAssertion(
                     context =>
                     {
                         if (context.User.HasClaim(x => x.Type == "edit")) {
                             return true;
                         }
                         return false;
                     }
                 ));

                option.AddPolicy("音乐编辑2", p => p.AddRequirements(new userQueirment()));

            });

            //services.AddSingleton<IAuthorizationHandler, EmailHandler>();
            services.AddSingleton<IAuthorizationHandler, userQueirmentHandler>();
            // services.AddSingleton<IAuthorizationHandler, zyz2Handler>();

            services.AddResponseCompression();

            services.AddAntiforgery();
            services.AddMvc(
                option => {
                    option.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    //option.Filters.Add(new LogResourceFilter("这里是全局"));
                    //option.Filters.Add(typeof(LogResourceFilter));
                    //option.Filters.Add<LogResourceFilter>();

                    option.CacheProfiles.Add("defalut", new CacheProfile() {
                        Duration = 60
                    });
                    option.CacheProfiles.Add("never", new CacheProfile()
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore=true
                    });
                    
                }                
            );

            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                //显示异常的详细信息
                app.UseDeveloperExceptionPage();
                //显示400-599的状态码
                app.UseStatusCodePages();
                //这个什么都不显示
                app.UseDatabaseErrorPage();
            }
            else
            {
                //到指定的页面这个页面在Shared文件夹中，也可以自定义一个
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
