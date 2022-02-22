using Hospha.DbModel;
using Hospha.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hospha.Api
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
            services.AddAuthorization(options =>
                {
                    options.AddPolicy(Model.Constants.Roles.admin, policy =>
                    {
                        policy.RequireClaim("role", Model.Constants.Roles.admin);
                    });

                    options.AddPolicy(Model.Constants.Roles.staff, policy =>
                    {
                        policy.RequireClaim("role", Model.Constants.Roles.staff);
                    });
                });

            services.AddAuthentication("Bearer")
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = "https://localhost:5000";
                        options.ApiName = "hosphaapi";
                        //options.ApiSecret = "supersecret";
                    });
            //.AddJwtBearer("Bearer", options =>
            //{
            //    options.Authority = "https://localhost:5000";
            //    options.SaveToken = true;
            //    options.Audience = "hosphaapiclient";
            //    options
            //});




            services.AddControllers();

            services.AddDbContext<HosphaContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hospha.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hospha.Api v1"));
            }


            app.UseHttpsRedirection();

            app.UseStatusCodePages(async context =>
           {
               if (context.HttpContext.Response.StatusCode == 401)
               {
                   context.HttpContext.Response.ContentType = "text/plain";
                   await context.HttpContext.Response.WriteAsync("Custom Unauthorized request");
               }
           });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
