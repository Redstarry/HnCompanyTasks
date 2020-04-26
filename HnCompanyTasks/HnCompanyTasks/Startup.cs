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
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            services.AddAutoMapper(typeof(Startup));
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
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = "JwtBearToken"; //默认的身份验证模式
                options.DefaultChallengeScheme = "JwtBearToken";
                options.DefaultScheme = "JwtBearToken"; //默认模式

            }).AddJwtBearer("JwtBearToken", options => {
                //options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    SaveSigninToken = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("123456789963258741")),

                    ValidateIssuer = true,
                    ValidIssuer = "tp",

                    ValidateAudience = true,
                    ValidAudience = "everyone",

                    ValidateLifetime = true,

                    //ClockSkew = TimeSpan.FromMinutes(5)

                };

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

            app.UseAuthentication();
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
