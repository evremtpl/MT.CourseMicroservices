using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MT.FreeCourse.Catalog.Services.Concrete;
using MT.FreeCourse.Catalog.Services.Interfaces;
using MT.FreeCourse.Catalog.Settings.Abstract;
using MT.FreeCourse.Catalog.Settings.Concrete;

namespace MT.FreeCourse.Catalog
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

            #region DI
            services.AddScoped<ICategoryService, CategoryService>();
            #endregion

            #region SwaggerDependencies

            services.AddSwaggerGen(c=> {
                c.SwaggerDoc("v1", new OpenApiInfo {
                 Title=" MT.FreeCourse.Catalog",
                 Version="v1"
                
                });
            
            });
            #endregion
            #region AutoMapperDependencies
            services.AddAutoMapper(typeof(Startup) );
            #endregion

            #region OptionPattern
            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));

            services.AddSingleton<IDatabaseSettings>(sp => {
                return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;

            });
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(opt=> {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json"," M.T.");
            
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
