using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Plark.Context;
using Plark.UnitOfWorks;
using Plark.UnitOfWorkInterfaces;
using Plark.Managers;
using Plark.Managers.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Plark.Hubs;
using System.Net;

namespace Plark
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
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailManager, EmailManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<ICookieManager, CookieManager>();
            services.AddScoped<ITicketManager, TicketManager>();

            services.AddControllers();
            services.AddMvcCore(opt =>
            {
                opt.EnableEndpointRouting = false;
            });

            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Audience = "plarkMobile";
                    options.ClaimsIssuer = "https://*:5001";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        ValidateIssuer = false,
                        ValidateAudience = true
                    };
                });

            services.AddDbContextPool<PlarkContext>(
                opt => opt.UseSqlServer(Configuration.GetConnectionString("PlarkDbConnectionString")));

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

                await next();
            });

            app.UseCors(options => options.SetIsOriginAllowed(x => _ = true)
                .AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.UseCors("AllowAll");

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseMvc(opt =>
            {
                opt.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{userId}/{id}"
                    );
            });

            app.UseEndpoints(routes =>
            {
                routes.MapHub<TicketsHub>("/hubs/tickets", (options) =>
                {
                    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
                });
            });
        }
    }
}
