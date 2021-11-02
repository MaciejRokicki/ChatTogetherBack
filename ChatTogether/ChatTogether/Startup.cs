using ChatTogether.Commons.ConfigurationModels;
using ChatTogether.Hubs;
using ChatTogether.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using SimpleInjector;
using System.IO;

namespace ChatTogether
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly Container container = new Container();

        public Startup(IWebHostEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterMvcAndCors(configuration);
            services.RegisterAuthentication();
            services.RegisterSignalR();
            services.RegisterAutomapper();
            services.RegisterDbContexts(configuration);
            services.RegisterSwagger();
            services.RegisterSimpleInjector(container);
            services.RegisterConfiguration(configuration);

            services.AddControllers();

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = configuration.GetValue<long>("StaticFiles:MaxFilesSize");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CORS");
            app.UseSimpleInjectorExtension(container);
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatTogether API V1");
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<InformationHub>("/informationHub");
                endpoints.MapHub<RoomHub>("/roomHub");
            });

            string staticFilesPath = configuration.GetValue<string>("StaticFiles:Path");
            string frontendUrl = configuration.GetValue<string>("Frontend:URL");

            if (!Directory.Exists(staticFilesPath))
            {
                Directory.CreateDirectory(staticFilesPath);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, staticFilesPath)),
                RequestPath = $"/{staticFilesPath}",
                OnPrepareResponse = ctx => {
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", frontendUrl);
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
                },
            });
        }
    }
}
