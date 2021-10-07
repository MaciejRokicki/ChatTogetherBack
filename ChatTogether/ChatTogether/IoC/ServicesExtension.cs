using ChatTogether.Commons.ConfigurationModels;
using ChatTogether.Commons.EmailSender;
using ChatTogether.Commons.RandomStringGenerator;
using ChatTogether.Dal;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Threading.Tasks;

namespace ChatTogether.IoC
{
    public static class ServicesExtension
    {
        public static void RegisterDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChatTogetherDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public static void RegisterAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(55);
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Events.OnRedirectToLogin = ctxt =>
                    {
                        ctxt.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                });

            services.AddHttpContextAccessor();
        }

        public static void RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatTogether API", Version = "v1" });
            });
        }

        public static void RegisterMvcAndCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CORS",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        public static void RegisterSignalR(this IServiceCollection services)
        {
            services.AddSignalR();
        }

        public static void RegisterAutomapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
        }

        public static void RegisterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FrontendConfiguration>(configuration.GetSection("Frontend"));
            services.Configure<RandomStringGeneratorConfiguration>(configuration.GetSection("RandomStringGenerator"));
            services.Configure<EmailConfiguration>(configuration.GetSection("Email"));
        }
    }
}
