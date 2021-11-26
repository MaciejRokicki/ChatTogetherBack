using ChatTogether.Commons.EmailSender;
using ChatTogether.Commons.ImageService;
using ChatTogether.Commons.RandomStringGenerator;
using ChatTogether.Dal.Interfaces;
using ChatTogether.Dal.Interfaces.Security;
using ChatTogether.Dal.Repositories;
using ChatTogether.Dal.Repositories.Security;
using ChatTogether.FluentValidator.Validators;
using ChatTogether.FluentValidator.Validators.Security;
using ChatTogether.Logic.Interfaces.MemoryStores;
using ChatTogether.Logic.Interfaces.Services;
using ChatTogether.Logic.Interfaces.Services.Security;
using ChatTogether.Logic.MemoryStores;
using ChatTogether.Logic.Services;
using ChatTogether.Logic.Services.Security;
using SimpleInjector;

namespace ChatTogether.IoC
{
    public static class ServicesRegistrations
    {
        public static void RegisterRepositories(this Container container)
        {
            container.Register<IAccountRepository, AccountRepository>();
            container.Register<IBlockedAccountRepository, BlockedAccountRepository>();
            container.Register<IConfirmEmailTokenRepository, ConfirmEmailTokenRepository>();
            container.Register<IChangeEmailTokenRepository, ChangeEmailTokenRepository>();
            container.Register<IChangePasswordTokenRepository, ChangePasswordTokenRepository>();

            container.Register<IUserRepository, UserRepository>();
            container.Register<IRoomRepository, RoomRepository>();
            container.Register<IMessageRepository, MessageRepository>();
        }

        public static void RegisterServices(this Container container)
        {
            container.Register<IEncryptionService, EncryptionService>();
            container.Register<ISecurityService, SecurityService>();

            container.Register<IUserService, UserService>();
            container.Register<IRoomService, RoomService>();
            container.Register<IMessageService, MessageService>();
        }

        public static void RegisterCommons(this Container container)
        {
            container.Register<IRandomStringGenerator, RandomStringGenerator>();
            container.Register<IEmailSender, EmailSender>();
            container.Register<IImageService, ImageService>();
        }

        public static void RegisterValidators(this Container container)
        {
            container.Register<RegistraionModelValidator>();
            container.Register<LoginModelValidator>();

            container.Register<UserModelValidator>();
            container.Register<FileToUploadModelValidator>();
        }

        public static void RegisterMemoryStores(this Container container)
        {
            container.Register<IRoomMemoryStore, RoomMemoryStore>(Lifestyle.Singleton);
            container.Register<IUserMemoryStore, UserMemoryStore>(Lifestyle.Singleton);
            container.Register<IMessageMemoryStore, MessageMemoryStore>(Lifestyle.Singleton);
        }
    }
}
