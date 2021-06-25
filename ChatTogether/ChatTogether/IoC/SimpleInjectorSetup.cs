using ChatTogether.Commons.SimpleInjector;
using ChatTogether.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ChatTogether.IoC
{
    public static class SimpleInjectorSetup
    {
        public static void RegisterSimpleInjector(this IServiceCollection services, Container container)
        {
            services.AddSimpleInjector(container, options =>
            {
                options.AddAspNetCore()
                    .AddControllerActivation();
            });

            services.RegisterHub(container, typeof(RoomHub).Assembly);
        }

        public static void UseSimpleInjectorExtension(this IApplicationBuilder app, Container container)
        {
            app.UseSimpleInjector(container);
            InitializeContainer(container);

            container.Verify();
        }

        private static void InitializeContainer(Container container)
        {
            container.RegisterRepositories();
            container.RegisterServices();
            container.RegisterCommons();
            container.RegisterValidators();
        }

        private static void RegisterHub(this IServiceCollection services, Container container, Assembly assembly)
        {
            IEnumerable<Type> types = container.GetTypesToRegister<Hub>(assembly);

            foreach (Type type in types)
                container.Register(type, type, Lifestyle.Scoped);

            services.AddScoped(typeof(IHubActivator<>), typeof(SimpleInjectorHubActivator<>));
        }
    }
}
