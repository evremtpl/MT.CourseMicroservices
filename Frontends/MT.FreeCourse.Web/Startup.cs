using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MT.FreeCourse.Web.Handlers;
using MT.FreeCourse.Web.Services.Concrete;
using MT.FreeCourse.Web.Services.Interfaces;
using MT.FreeCourse.Web.Settings;
using System;

namespace MT.FreeCourse.Web
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

            #region AppSettingConfClass
            services.Configure<ServiceApiSettings>(Configuration.GetSection("ServiceApiSettings"));

            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));
            #endregion
            var serviceApiSettings = Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();
            services.AddHttpContextAccessor();
            services.AddHttpClient<IIdentityService, IdentityService>();


            services.AddScoped<PasswordTokenHandler>();
            services.AddHttpClient<IUserService, UserService>(opt=>
            {
                opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
            
            }).AddHttpMessageHandler<PasswordTokenHandler>();
         

            #region CookieConfiguration
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie
                (CookieAuthenticationDefaults.AuthenticationScheme, "MerveCookies", opt =>
                  {
                      opt.LoginPath = "/Auth/SignIn";
                      opt.ExpireTimeSpan = TimeSpan.FromDays(60);
                      opt.SlidingExpiration = true;
                      opt.Cookie.Name = "FreeCourseWebCookie";
                  });



            #endregion






            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
