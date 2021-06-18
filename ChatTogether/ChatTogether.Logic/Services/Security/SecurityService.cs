using ChatTogether.Commons.ConfigurationModels;
using ChatTogether.Commons.EmailSender;
using ChatTogether.Commons.EmailSender.Models.Templates;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Interfaces.Security;
using ChatTogether.Logic.Interfaces.Security;
using ChatTogether.Ports.Dtos.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Services.Security
{
    public class SecurityService : ISecurityService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IEncryptionService encryptionService;
        private readonly IConfirmEmailService confirmEmailService; 
        private readonly IChangeEmailService changeEmailService;
        private readonly IChangePasswordService changePasswordService;
        private readonly IEmailSender emailSender;
        private readonly FrontendConfiguration frontendConfiguration;

        public SecurityService(
            IAccountRepository accountRepository,
            IEncryptionService encryptionService, 
            IConfirmEmailService confirmEmailService, 
            IChangeEmailService changeEmailService, 
            IChangePasswordService changePasswordService, 
            IEmailSender emailSender, 
            IOptions<FrontendConfiguration> frontendConfiguration
            )
        {
            this.accountRepository = accountRepository;
            this.encryptionService = encryptionService;
            this.confirmEmailService = confirmEmailService;
            this.changeEmailService = changeEmailService;
            this.changePasswordService = changePasswordService;
            this.emailSender = emailSender;
            this.frontendConfiguration = frontendConfiguration.Value;
        }

        public async Task ChangeEmail(string token, string newEmail)
        {
            ChangeEmailTokenDbo changeEmailTokenDbo = await changeEmailService.Get(token);

            if (changeEmailTokenDbo == null)
            {
                return;
            }

            AccountDbo accountDbo = changeEmailTokenDbo.Account;

            bool result = await changeEmailService.CheckToken(accountDbo.Id, token);

            if (result)
            {
                accountDbo.Email = newEmail;
                accountDbo.IsConfirmed = false;
                accountDbo = await accountRepository.UpdateAsync(accountDbo);
                await SendConfirmationEmail(accountDbo.Email);

                await changeEmailService.DeleteToken(accountDbo.Id);
            }
        }

        public async Task ChangePassword(string token, string newPassword)
        {
            ChangePasswordTokenDbo changePasswordTokenDbo = await changePasswordService.Get(token);

            if (changePasswordTokenDbo == null)
            {
                return;
            }

            AccountDbo accountDbo = changePasswordTokenDbo.Account;

            bool result = await changePasswordService.CheckToken(accountDbo.Id, token);

            if (result)
            {
                accountDbo.Password = encryptionService.EncryptionSHA256(accountDbo.Password);
                accountDbo = await accountRepository.UpdateAsync(accountDbo);

                await changePasswordService.DeleteToken(accountDbo.Id);
            }
        }

        public async Task ConfirmEmail(string email, string token)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == email);

            if (accountDbo == null)
            {
                return;
            }

            bool result = await confirmEmailService.CheckToken(accountDbo.Id, token);

            if(result)
            {
                accountDbo.IsConfirmed = true;
                await accountRepository.UpdateAsync(accountDbo);

                await confirmEmailService.DeleteToken(accountDbo.Id);
            }
        }

        public async Task ResendConfirmationEmail(string email)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == email);

            if(accountDbo == null)
            {
                return;
            }

            await confirmEmailService.DeleteToken(accountDbo.Id);
            await SendConfirmationEmail(email);
        }

        private async Task SendConfirmationEmail(string email)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == email);

            if (accountDbo == null)
            {
                return;
            }

            ConfirmEmailTokenDbo confirmEmailTokenDbo = await confirmEmailService.CreateToken(accountDbo.Id);

            string url = string.Format("{0}/confirmEmail?email={1}&token={2}", frontendConfiguration.URL, email, confirmEmailTokenDbo.Token);
            await emailSender.Send(email, new ConfirmRegistrationTemplate(email, url));
        }

        public async Task SendRequestToChangeEmail(string email)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == email);

            if (accountDbo == null)
            {
                return;
            }

            ChangeEmailTokenDbo changeEmailTokenDbo = await changeEmailService.CreateToken(accountDbo.Id);

            string url = string.Format("{0}/changeEmail?email={1}&token={2}", frontendConfiguration.URL, email, changeEmailTokenDbo.Token);
            await emailSender.Send(email, new ChangeEmailRequestTemplate(email, url));
        }

        public async Task SendRequestToChangePassword(string email)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == email);

            if (accountDbo == null)
            {
                return;
            }

            ChangePasswordTokenDbo changePasswordTokenDbo = await changePasswordService.CreateToken(accountDbo.Id);

            string url = string.Format("{0}/changePassword?email={1}&token={2}", frontendConfiguration.URL, email, changePasswordTokenDbo.Token);
            await emailSender.Send(email, new ChangePasswordRequestTemplate(email, url));
        }

        public async Task<ClaimsPrincipal> SignIn(AccountDto accountDto)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == accountDto.Email);

            if(accountDbo == null)
            {
                return null;
            }

            if(!accountDbo.IsConfirmed)
            {
                return null;
            }

            bool isCorrect = encryptionService.VerifySHA256(accountDto.Password, accountDbo.Password);

            if(!isCorrect)
            {
                return null;
            }

            List<Claim> claims = GetClaims(accountDto.Email);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }

        public async Task SignUp(AccountDto accountDto, string nickname)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == accountDto.Email);

            if(accountDbo != null)
            {
                return;
            }

            accountDbo = new AccountDbo()
            {
                Email = accountDto.Email,
                Password = encryptionService.EncryptionSHA256(accountDto.Password)
            };

            accountDbo = await accountRepository.CreateAsync(accountDbo);
            await SendConfirmationEmail(accountDbo.Email);
        }

        private List<Claim> GetClaims(string email)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email)
            };
        }
    }
}
