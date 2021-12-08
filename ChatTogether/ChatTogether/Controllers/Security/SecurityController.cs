using AutoMapper;
using ChatTogether.Commons.Exceptions;
using ChatTogether.Commons.HubInvokeResults;
using ChatTogether.Commons.Page;
using ChatTogether.Commons.Role;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.FluentValidation.Validators.Security;
using ChatTogether.Hubs;
using ChatTogether.Hubs.Interfaces;
using ChatTogether.Logic.Interfaces.MemoryStores;
using ChatTogether.Logic.Interfaces.Services;
using ChatTogether.Ports.Dtos.Security;
using ChatTogether.Ports.HubModels;
using ChatTogether.ViewModels;
using ChatTogether.ViewModels.Security;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatTogether.Controllers.Security
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly ISecurityService securityService;
        private readonly IUserService userService;
        private readonly LoginModelValidator loginModelValidator;
        private readonly RegistraionModelValidator registrationModelValidator;
        private readonly IUserMemoryStore userMemoryStore;
        private readonly IHubContext<InformationHub, IInformationHub> informationHub;

        public SecurityController(
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            ISecurityService securityService,
            IUserService userService,
            LoginModelValidator loginModelValidator,
            RegistraionModelValidator registrationModelValidator,
            IHubContext<InformationHub, IInformationHub> informationHub, 
            IUserMemoryStore userMemoryStore)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.securityService = securityService;
            this.userService = userService;
            this.loginModelValidator = loginModelValidator;
            this.registrationModelValidator = registrationModelValidator;
            this.informationHub = informationHub;
            this.userMemoryStore = userMemoryStore;
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> ChangeEmail(string token, string newEmail)
        {
            try
            {
                ValidationResult validationResult = await loginModelValidator.ValidateAsync(new LoginModel() { Email = newEmail, Password = "strinG1!" });

                if (!validationResult.IsValid)
                {
                    throw new InvalidDataException();
                }

                await securityService.ChangeEmailAsync(token, newEmail);
                return Ok();
            }
            catch(IncorrectDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(EmailExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> ChangePassword(string token, [FromBody] string newPassword)
        {
            try
            {
                ValidationResult validationResult = await loginModelValidator.ValidateAsync(new LoginModel() { Email = "example@example.com", Password = newPassword });

                if (!validationResult.IsValid)
                {
                    throw new InvalidDataException();
                }

                await securityService.ChangePasswordAsync(token, newPassword);

                return Ok();
            }
            catch (IncorrectDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                ValidationResult validationResult = await loginModelValidator.ValidateAsync(new LoginModel() { Email = email, Password = "strinG1!" });

                if (!validationResult.IsValid)
                {
                    throw new InvalidDataException();
                }

                await securityService.ChangePasswordRequestAsync(email);

                return Ok();
            }
            catch (IncorrectDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            try
            {
                await securityService.ConfirmEmailAsync(email, token);

                return Ok();
            }
            catch (IncorrectDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            try
            {
                ValidationResult validationResult = await loginModelValidator.ValidateAsync(new LoginModel() { Email = email, Password = "strinG1!" });

                if (!validationResult.IsValid)
                {
                    throw new InvalidDataException();
                }

                await securityService.SendConfirmationEmailAsync(email);

                return Ok();
            }
            catch (IncorrectDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> ChangeEmailRequest()
        {
            try
            {
                string email = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                await securityService.ChangeEmailRequestAsync(email);

                return Ok();
            }
            catch (IncorrectDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> ChangePasswordRequest()
        {
            try
            {
                string email = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                await securityService.ChangePasswordRequestAsync(email);

                return Ok();
            }
            catch (IncorrectDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignIn([FromBody] LoginModel loginModel)
        {
            try
            {
                ValidationResult validationResult = await loginModelValidator.ValidateAsync(loginModel);

                if (!validationResult.IsValid)
                {
                    throw new InvalidDataException();
                }

                AccountDto accountDto = mapper.Map<AccountDto>(loginModel);
                (ClaimsPrincipal claimsPrincipal, UserDbo userDbo) = await securityService.SignInAsync(accountDto);

                await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                UserViewModel userViewModel = mapper.Map<UserViewModel>(userDbo);

                return Ok(userViewModel);
            }
            catch (IncorrectDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (AccountUnconfirmedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(BlockedAccountException ex)
            {
                return BadRequest(new Dictionary<string, object>()
                {
                    { "message", ex.Message },
                    { "data", ex.Data["BlockedAccount"] },
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignUp([FromBody] RegistrationModel registrationModel)
        {
            try
            {
                ValidationResult validationResult = await registrationModelValidator.ValidateAsync(registrationModel);

                if (!validationResult.IsValid)
                {
                    throw new InvalidDataException();
                }

                AccountDto accountDto = mapper.Map<AccountDto>(registrationModel);
                await securityService.SignUpAsync(accountDto, registrationModel.Nickname);

                return Ok();
            }
            catch (EmailExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NicknameExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            try
            {
                await httpContextAccessor.HttpContext.SignOutAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> Validate()
        {
            try
            {
                string nickname = httpContextAccessor.HttpContext.User.FindFirstValue("Nickname");

                UserDbo userDbo = await userService.GetUserAsync(nickname);
                UserViewModel userViewModel = mapper.Map<UserViewModel>(userDbo);

                return Ok(userViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        [AuthorizeRoles(Role.ADMINISTRATOR)]
        public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleModel changeRoleViewModel)
        {
            try
            {
                await securityService.ChangeRoleAsync(changeRoleViewModel.UserId, changeRoleViewModel.Role);

                UserHubModel userHubModel = userMemoryStore.GetUser(changeRoleViewModel.UserId);

                if (userHubModel != null)
                {
                    await informationHub.Clients.Client(userHubModel.ConnectionId).Signout(SignoutInvokeResults.ROLE_CHANGED);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [AuthorizeRoles(Role.MODERATOR, Role.ADMINISTRATOR)]
        public async Task<IActionResult> GetBlockedUsers(int page = 1, string search = "")
        {
            try
            {
                Page<BlockedAccountDbo> blockedUsers = await securityService.GetBlockedUsersAsync(page, 10, search);

                Page<BlockedAccountViewModel> result = mapper.Map<Page<BlockedAccountViewModel>>(blockedUsers);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        [AuthorizeRoles(Role.MODERATOR, Role.ADMINISTRATOR)]
        public async Task<IActionResult> BlockUser([FromBody] BlockAccountModel blockAccountViewModel)
        {
            try
            {
                string nickname = httpContextAccessor.HttpContext.User.FindFirstValue("Nickname");
                UserDbo userDbo = await userService.GetUserAsync(nickname);

                bool logout = await securityService.BlockAccountAsync(blockAccountViewModel.UserId, blockAccountViewModel.Reason, userDbo.Account.Id, blockAccountViewModel.BlockedTo);

                if (logout)
                {
                    UserHubModel userHubModel = userMemoryStore.GetUser(blockAccountViewModel.UserId);

                    if (userHubModel != null)
                    {
                        await informationHub.Clients.Client(userHubModel.ConnectionId).Signout(SignoutInvokeResults.ACCOUNT_BLOCKED);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        [AuthorizeRoles(Role.MODERATOR, Role.ADMINISTRATOR)]
        public async Task<IActionResult> UnblockUser([FromBody] int userId)
        {
            try
            {
                await securityService.UnblockAccountAsync(userId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
