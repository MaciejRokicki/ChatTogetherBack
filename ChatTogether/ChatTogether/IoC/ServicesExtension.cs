using ChatTogether.Commons.EmailSender;
using ChatTogether.Commons.RandomStringGenerator;
using ChatTogether.Dal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

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
            // TODO: zarejestrowac uwierzytelnianie
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
                        builder.WithOrigins("http://localhost:4200")
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
            services.Configure<RandomStringGeneratorConfiguration>(configuration.GetSection("RandomStringGeneratorConfiguration"));
            services.Configure<EmailConfiguration>(configuration.GetSection("EmailConfiguration"));
        }
    }
}
