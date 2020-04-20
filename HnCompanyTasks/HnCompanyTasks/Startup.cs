using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HnCompanyTasks.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;

namespace HnCompanyTasks
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
            services.AddTransient<ITasksServer, TasksServer>();
            services.AddTransient<ISchedulerFactory, StdSchedulerFactory>();
            services.AddCors(option => option.AddPolicy("Domain", builder=>builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddSwaggerGen(option => {
                option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {

                    Title = "TasksManager",
                    Version = "V1"
                }) ;
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var xmlPath = Path.Combine(basePath, "SwaggerDemo.xml");
                option.IncludeXmlComments(xmlPath);
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
            app.UseCors("Domain");

            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(option=> {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "TasksManager API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
