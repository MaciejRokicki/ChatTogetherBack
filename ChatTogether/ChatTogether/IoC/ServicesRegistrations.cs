using ChatTogether.Commons.EmailSender;
using ChatTogether.Commons.RandomStringGenerator;
using ChatTogether.Dal.Interfaces;
using ChatTogether.Dal.Repositories;
using ChatTogether.FluentValidator;
using ChatTogether.Logic.Interfaces;
using ChatTogether.Logic.Services;
using SimpleInjector;

namespace ChatTogether.IoC
{
    public static class ServicesRegistrations
    {
        public static void RegisterRepositories(this Container container)
        {
            container.Register<IExampleRepository, ExampleRepository>();
        }

        public static void RegisterServices(this Container container)
        {
            container.Register<IExampleService, ExampleService>();
        }

        public static void RegisterCommons(this Container container)
        {
            container.Register<IRandomStringGenerator, RandomStringGenerator>();
            container.Register<IEmailSender, EmailSender>();
        }

        public static void RegisterValidators(this Container container)
        {
            container.Register<ExampleValidator>();
        }
    }
}
