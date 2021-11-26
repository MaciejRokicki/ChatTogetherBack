using ChatTogether.Commons.ConfigurationModels;
using ChatTogether.Commons.EmailSender;
using ChatTogether.Commons.EmailSender.Models.Templates;
using ChatTogether.Commons.Exceptions;
using ChatTogether.Commons.Page;
using ChatTogether.Commons.RandomStringGenerator;
using ChatTogether.Commons.Role;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Interfaces.Security;
using ChatTogether.Logic.Interfaces.Services;
using ChatTogether.Logic.Interfaces.Services.Security;
using ChatTogether.Ports.Dtos.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Services.Security
{
    public class SecurityService : ISecurityService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IBlockedAccountRepository blockedAccountRepository;
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
            IBlockedAccountRepository blockedAccountRepository,
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
            this.blockedAccountRepository = blockedAccountRepository;
            this.encryptionService = encryptionService;
            this.userService = userService;
            this.confirmEmailTokenRepository = confirmEmailTokenRepository;
            this.changeEmailTokenRepository = changeEmailTokenRepository;
            this.changePasswordTokenRepository = changePasswordTokenRepository;
            this.emailSender = emailSender;
            this.frontendConfiguration = frontendConfiguration.Value;
            this.randomStringGenerator = randomStringGenerator;
        }

        public async Task ChangeEmailAsync(string token, string newEmail)
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
            await SendConfirmationEmailAsync(accountDbo.Email, false);
            await changeEmailTokenRepository.DeleteAsync(x => x.Id == changeEmailTokenDbo.Id);
        }

        public async Task ChangePasswordAsync(string token, string newPassword)
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

        public async Task ConfirmEmailAsync(string email, string token)
        {
            ConfirmEmailTokenDbo confirmationToken = await confirmEmailTokenRepository.GetWithAccountAsync(x => x.Token == token && x.Account.Email == email);

            if (confirmationToken == null)
            {
                throw new IncorrectDataException();
            }

            confirmationToken.Account.IsConfirmed = true;
            confirmationToken.Account.ConfirmEmailTokenDbo = null;

            await accountRepository.UpdateAsync(confirmationToken.Account);
            await confirmEmailTokenRepository.DeleteAsync(x => x.Token == token);
        }

        public async Task SendConfirmationEmailAsync(string email, bool isNewAccount = true)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == email);

            if (accountDbo == null)
            {
                throw new IncorrectDataException();
            }

            ConfirmEmailTokenDbo confirmEmailTokenDbo = await confirmEmailTokenRepository.GetAsync(x => x.AccountId == accountDbo.Id);

            if(confirmEmailTokenDbo != null)
            {
                await confirmEmailTokenRepository.DeleteAsync(x => x.AccountId == accountDbo.Id);
            }

            confirmEmailTokenDbo = new ConfirmEmailTokenDbo()
            {
                AccountId = accountDbo.Id,
                Token = randomStringGenerator.Generate(RandomStringType.Token)
            };

            confirmEmailTokenDbo = await confirmEmailTokenRepository.CreateAsync(confirmEmailTokenDbo);

            if (isNewAccount)
            {
                string url = string.Format("{0}/#/security/email/confirm/{1}/{2}", frontendConfiguration.URL, email, confirmEmailTokenDbo.Token);
                await emailSender.Send(email, new ConfirmRegistrationTemplate(email, url));
            }
            else
            {
                string url = string.Format("{0}/#/security/email/confirm/{1}/{2}", frontendConfiguration.URL, email, confirmEmailTokenDbo.Token);
                await emailSender.Send(email, new ConfirmChangeEmailTemplate(email, url));
            }
        }

        public async Task ChangeEmailRequestAsync(string email)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == email);

            if (accountDbo == null)
            {
                throw new IncorrectDataException();
            }

            ChangeEmailTokenDbo changeEmailTokenDbo = await changeEmailTokenRepository.GetAsync(x => x.AccountId == accountDbo.Id);

            if (changeEmailTokenDbo != null)
            {
                await changeEmailTokenRepository.DeleteAsync(x => x.AccountId == accountDbo.Id);
            }

            changeEmailTokenDbo = new ChangeEmailTokenDbo()
            {
                AccountId = accountDbo.Id,
                Token = randomStringGenerator.Generate(RandomStringType.Token)
            };

            changeEmailTokenDbo = await changeEmailTokenRepository.CreateAsync(changeEmailTokenDbo);

            string url = string.Format("{0}/#/security/email/change/{1}/{2}", frontendConfiguration.URL, email, changeEmailTokenDbo.Token);
            await emailSender.Send(email, new ChangeEmailRequestTemplate(email, url));
        }

        public async Task ChangePasswordRequestAsync(string email)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == email);

            if (accountDbo == null)
            {
                throw new IncorrectDataException();
            }

            ChangePasswordTokenDbo changePasswordTokenDbo = await changePasswordTokenRepository.GetAsync(x => x.AccountId == accountDbo.Id);

            if(changePasswordTokenDbo != null)
            {
                await changePasswordTokenRepository.DeleteAsync(x => x.AccountId == accountDbo.Id);
            }

            changePasswordTokenDbo = new ChangePasswordTokenDbo()
            {
                AccountId = accountDbo.Id,
                Token = randomStringGenerator.Generate(RandomStringType.Token)
            };

            changePasswordTokenDbo = await changePasswordTokenRepository.CreateAsync(changePasswordTokenDbo);

            string url = string.Format("{0}/#/security/password/change/{1}/{2}", frontendConfiguration.URL, email, changePasswordTokenDbo.Token);
            await emailSender.Send(email, new ChangePasswordRequestTemplate(email, url));
        }

        public async Task<(ClaimsPrincipal, UserDbo)> SignInAsync(AccountDto accountDto)
        {
            AccountDbo accountDbo = await accountRepository.GetWithUserAsync(x => x.Email == accountDto.Email);

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

            if(accountDbo.BlockedAccountId != null)
            {
                if (accountDbo.BlockedAccountDbo.BlockedTo == null || accountDbo.BlockedAccountDbo.BlockedTo >= DateTime.UtcNow)
                {
                    throw new BlockedAccountException(accountDbo.BlockedAccountDbo.Reason, accountDbo.BlockedAccountDbo.Created, accountDbo.BlockedAccountDbo.BlockedTo);
                }
                else
                {
                    await UnblockAccountAsync(accountDbo.User.Id);
                }
            }

            List<Claim> claims = GetClaims(accountDbo.Email, accountDbo.Role, accountDbo.User.Id.ToString(), accountDbo.User.Nickname);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return (claimsPrincipal, accountDbo.User);
        }

        public async Task SignUpAsync(AccountDto accountDto, string nickname)
        {
            AccountDbo accountDbo = await accountRepository.GetAsync(x => x.Email == accountDto.Email);

            if (accountDbo != null)
            {
                throw new EmailExistsException();
            }

            bool isNicknameAvailable = await userService.IsNicknameAvailableAsync(nickname);

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
            await SendConfirmationEmailAsync(accountDbo.Email);
        }

        private List<Claim> GetClaims(string email, Role role, string userId, string nickname)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim("UserId", userId),
                new Claim("Nickname", nickname)
            };
        }

        public async Task ChangeRoleAsync(int userId, Role role)
        {
            AccountDbo accountDbo = await accountRepository.GetWithUserAsync(x => x.User.Id == userId);

            if (accountDbo == null)
            {
                return;
            }

            accountDbo.Role = role;

            await accountRepository.UpdateAsync(accountDbo);
        }

        public async Task<bool> BlockAccountAsync(int userId, string reason, int blockedById, DateTime? blockedTo = null)
        {
            AccountDbo accountDbo = await accountRepository.GetWithUserAsync(x => x.User.Id == userId);

            if (accountDbo == null)
            {
                return false;
            }

            if (accountDbo.Role == Role.ADMINISTRATOR || accountDbo.Role == Role.MODERATOR)
            {
                AccountDbo blockedBy = await accountRepository.GetWithUserAsync(x => x.Id == blockedById);

                if (blockedBy.Role == Role.MODERATOR)
                {
                    return false;
                }
            }

            if (accountDbo.BlockedAccountId != null)
            {
                await blockedAccountRepository.DeleteAsync(x => x.Id == accountDbo.BlockedAccountId);
            }

            accountDbo.BlockedAccountDbo = new BlockedAccountDbo()
            {
                Reason = reason,
                BlockedTo = blockedTo,
                CreatedById = blockedById
            };

            await accountRepository.UpdateAsync(accountDbo);

            return true;
        }

        public async Task UnblockAccountAsync(int userId)
        {
            AccountDbo accountDbo = await accountRepository.GetWithUserAsync(x => x.User.Id == userId);

            if(accountDbo == null)
            {
                return;
            }

            await blockedAccountRepository.DeleteAsync(x => x.Id == accountDbo.BlockedAccountId);
        }

        public async Task<Page<BlockedAccountDbo>> GetBlockedUsersAsync(int page, int pageSize, string search)
        {
            Page<BlockedAccountDbo> blockedUsers = await blockedAccountRepository.GetPageAsync(page, pageSize, search);

            return blockedUsers;
        }
    }
}
