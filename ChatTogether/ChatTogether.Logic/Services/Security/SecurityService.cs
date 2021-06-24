using ChatTogether.Commons.ConfigurationModels;
using ChatTogether.Commons.EmailSender;
using ChatTogether.Commons.EmailSender.Models.Templates;
using ChatTogether.Commons.Exceptions;
using ChatTogether.Commons.RandomStringGenerator;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Interfaces.Security;
using ChatTogether.Logic.Interfaces;
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
        private readonly IUserService userService;
        private readonly IConfirmEmailTokenRepository confirmEmailTokenRepository;
        private readonly IChangeEmailTokenRepository changeEmailTokenRepository;
        private readonly IChangePasswordTokenRepository changePasswordTokenRepository;
        private readonly IEmailSender emailSender;
        private readonly FrontendConfiguration frontendConfiguration;
        private readonly IRandomStringGenerator randomStringGenerator;

        public SecurityService(
            IAccountRepository accountRepository,
            IEncryptionService encryptionService,
            IUserService userService,
            IConfirmEmailTokenRepository confirmEmailTokenRepository,
            IChangeEmailTokenRepository changeEmailTokenRepository,
            IChangePasswordTokenRepository changePasswordTokenRepository,
            IEmailSender emailSender,
            IOptions<FrontendConfiguration> frontendConfiguration,
            IRandomStringGenerator randomStringGenerator)
        {
            this.accountRepository = accountRepository;
            this.encryptionService = encryptionService;
            this.userService = userService;
            this.confirmEmailTokenRepository = confirmEmailTokenRepository;
            this.changeEmailTokenRepository = changeEmailTokenRepository;
            this.changePasswordTokenRepository = changePasswordTokenRepository;
            this.emailSender = emailSender;
            this.frontendConfiguration = frontendConfiguration.Value;
            this.randomStringGenerator = randomStringGenerator;
        }

        public async Task ChangeEmail(string token, string newEmail)
        {
            ChangeEmailTokenDbo changeEmailTokenDbo = await changeEmailTokenRepository.GetWithAccountAsync(x => x.Token == token);

            if (changeEmailTokenDbo == null)
            {
                throw new IncorrectDataException();
            }

            AccountDbo newEmailExist = await accountRepository.GetAsync(x => x.Email == newEmail);

            if (newEmailExist != null)
            {
                throw new EmailExistsException();
            }

            AccountDbo accountDbo = changeEmailTokenDbo.Account;

            accountDbo.Email = newEmail;
            accountDbo.IsConfirmed = false;
            accountDbo.ChangeEmailTokenDbo = null;

            accountDbo = await accountRepository.UpdateAsync(accountDbo);
            await SendConfirmationEmail(accountDbo.Email);
            await changeEmailTokenRepository.DeleteAsync(x => x.Id == changeEmailTokenDbo.Id);
        }

        public async Task ChangePassword(string token, string newPassword)
        {
            ChangePasswordTokenDbo changePasswordTokenDbo = await changePasswordTokenRepository.GetWithAccountAsync(x => x.Token == token);

            if (changePasswordTokenDbo == null)
            {
                throw new IncorrectDataException();
            }

            AccountDbo accountDbo = changePasswordTokenDbo.Account;

            accountDbo.Password = encryptionService.EncryptionSHA256(newPassword);
            accountDbo.ChangePasswordTokenDbo = null;

            accountDbo = await accountRepository.UpdateAsync(accountDbo);
            await changePasswordTokenRepository.DeleteAsync(x => x.Id == changePasswordTokenDbo.Id);
        }

        public async Task ConfirmEmail(string email, string token)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == email);

            if (accountDbo == null)
            {
                throw new IncorrectDataException();
            }

            accountDbo.IsConfirmed = true;
            accountDbo.ConfirmEmailTokenDbo = null;

            await accountRepository.UpdateAsync(accountDbo);
            await confirmEmailTokenRepository.DeleteAsync(x => x.AccountId == accountDbo.Id);
        }

        public async Task ResendConfirmationEmail(string email)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == email);

            if (accountDbo == null)
            {
                throw new IncorrectDataException();
            }

            await confirmEmailTokenRepository.DeleteAsync(x => x.AccountId == accountDbo.Id);
            await SendConfirmationEmail(email);
        }

        private async Task SendConfirmationEmail(string email)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == email);

            if (accountDbo == null)
            {
                throw new IncorrectDataException();
            }

            ConfirmEmailTokenDbo confirmEmailTokenDbo = new ConfirmEmailTokenDbo()
            {
                AccountId = accountDbo.Id,
                Token = randomStringGenerator.Generate()
            };

            confirmEmailTokenDbo = await confirmEmailTokenRepository.CreateAsync(confirmEmailTokenDbo);

            string url = string.Format("{0}/confirmEmail?email={1}&token={2}", frontendConfiguration.URL, email, confirmEmailTokenDbo.Token);
            await emailSender.Send(email, new ConfirmRegistrationTemplate(email, url));
        }

        public async Task ChangeEmailRequest(string email)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == email);

            if (accountDbo == null)
            {
                throw new IncorrectDataException();
            }

            ChangeEmailTokenDbo changeEmailTokenDbo = new ChangeEmailTokenDbo()
            {
                AccountId = accountDbo.Id,
                Token = randomStringGenerator.Generate()
            };

            changeEmailTokenDbo = await changeEmailTokenRepository.CreateAsync(changeEmailTokenDbo);

            string url = string.Format("{0}/changeEmail?email={1}&token={2}", frontendConfiguration.URL, email, changeEmailTokenDbo.Token);
            await emailSender.Send(email, new ChangeEmailRequestTemplate(email, url));
        }

        public async Task ChangePasswordRequest(string email)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == email);

            if (accountDbo == null)
            {
                throw new IncorrectDataException();
            }

            ChangePasswordTokenDbo changePasswordTokenDbo = new ChangePasswordTokenDbo()
            {
                AccountId = accountDbo.Id,
                Token = randomStringGenerator.Generate()
            };

            changePasswordTokenDbo = await changePasswordTokenRepository.CreateAsync(changePasswordTokenDbo);

            string url = string.Format("{0}/changePassword?email={1}&token={2}", frontendConfiguration.URL, email, changePasswordTokenDbo.Token);
            await emailSender.Send(email, new ChangePasswordRequestTemplate(email, url));
        }

        public async Task<ClaimsPrincipal> SignIn(AccountDto accountDto)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == accountDto.Email);

            if (accountDbo == null)
            {
                throw new IncorrectDataException();
            }

            if (!accountDbo.IsConfirmed)
            {
                throw new AccountUnconfirmedException();
            }

            bool isCorrect = encryptionService.VerifySHA256(accountDto.Password, accountDbo.Password);

            if (!isCorrect)
            {
                throw new IncorrectDataException();
            }

            List<Claim> claims = GetClaims(accountDto.Email);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }

        public async Task SignUp(AccountDto accountDto, string nickname)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == accountDto.Email);

            if (accountDbo != null)
            {
                throw new EmailExistsException();
            }

            bool isNicknameAvailable = await userService.IsNicknameAvailable(nickname);

            if(!isNicknameAvailable)
            {
                throw new NicknameExistsException();
            }

            UserDbo userDbo = new UserDbo()
            {
                Nickname = nickname
            };

            accountDbo = new AccountDbo()
            {
                Email = accountDto.Email,
                Password = encryptionService.EncryptionSHA256(accountDto.Password),
                User = userDbo
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
