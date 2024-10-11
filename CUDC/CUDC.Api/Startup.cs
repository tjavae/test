using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CUDC.Api.Data;
using CUDC.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CUDC.Api
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

            services.AddDbContext<SurveyContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("SurveyContext"), opt => {
                    opt.CommandTimeout(int.Parse(Configuration["CommandTimeout"]));
                });
            });

            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<ISurveyService, SurveyService>();
            services.AddTransient<ISecurityService, SecurityService>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            bool.TryParse(Configuration["SwaggerEnabled"], out bool swaggerEnabled);
            if (swaggerEnabled)
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{Configuration["SwaggerRoutePrefix"]}/swagger/v1/swagger.json", "Web API V1");
                });
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
