using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;

namespace MT.FreeCourse.Discount
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

            services.AddControllers();

            #region JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {

                opt.Authority = Configuration["IdentityServerURL"];
                opt.Audience = "resource_discount";
                opt.RequireHttpsMetadata = false;

            });
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
            #endregion

            var requireauthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

          //  var requiredScopePolicy = new AuthorizationPolicyBuilder().RequireClaim("scope","discount_read");
            services.AddControllers(opt => {
                opt.Filters.Add(new AuthorizeFilter(requireauthorizePolicy));

            });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
