using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MT.FreeCourse.Order.Application.Handlers;
using MT.FreeCourse.Order.Infrastructure;
using MT.FreeCourse.Shared.Services.Concrete;
using MT.FreeCourse.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace MT.FreeCourse.Order.API
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
            services.AddMediatR(typeof(CreateOrderCommandHandler).Assembly);
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();

            services.AddHttpContextAccessor();


            services.AddDbContext<OrderDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),configure=> {
                    configure.MigrationsAssembly("MT.FreeCourse.Order.Infrastructure");
                });

            });

            #region JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {

                opt.Authority = Configuration["IdentityServerURL"];
                opt.Audience = "resource_order";
                opt.RequireHttpsMetadata = false;

            });
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
            #endregion

            var requireauthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            
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
