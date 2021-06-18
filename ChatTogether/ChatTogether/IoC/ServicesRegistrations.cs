﻿using ChatTogether.Commons.EmailSender;
using ChatTogether.Commons.RandomStringGenerator;
using ChatTogether.Dal.Interfaces.Security;
using ChatTogether.Dal.Repositories.Security;
using ChatTogether.FluentValidator.Validators.Security;
using ChatTogether.Logic.Interfaces.Security;
using ChatTogether.Logic.Services.Security;
using SimpleInjector;

namespace ChatTogether.IoC
{
    public static class ServicesRegistrations
    {
        public static void RegisterRepositories(this Container container)
        {
            container.Register<IAccountRepository, AccountRepository>();
            container.Register<IConfirmEmailTokenRepository, ConfirmEmailTokenRepository>();
            container.Register<IChangeEmailTokenRepository, ChangeEmailTokenRepository>();
            container.Register<IChangePasswordTokenRepository, ChangePasswordTokenRepository>();
        }

        public static void RegisterServices(this Container container)
        {
            container.Register<IEncryptionService, EncryptionService>();
            container.Register<ISecurityService, SecurityService>();
            container.Register<IConfirmEmailService, ConfirmEmailService>();
            container.Register<IChangeEmailService, ChangeEmailService>();
            container.Register<IChangePasswordService, ChangePasswordService>();
        }

        public static void RegisterCommons(this Container container)
        {
            container.Register<IRandomStringGenerator, RandomStringGenerator>();
            container.Register<IEmailSender, EmailSender>();
        }

        public static void RegisterValidators(this Container container)
        {
            container.Register<RegistraionModelValidator>();
            container.Register<LoginModelValidator>();
        }
    }
}
