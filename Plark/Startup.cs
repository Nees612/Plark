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

            services.AddControllers();
            services.AddMvcCore(opt =>
            {
                opt.EnableEndpointRouting = false;
            });

            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Audience = "http://localhost:4200/";
                    options.ClaimsIssuer = "http://localhost:4200/";
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMvc(opt =>
            {
                opt.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id}"
                    );
            });
        }
    }
}
