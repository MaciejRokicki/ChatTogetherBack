using AutoMapper;
using ChatTogether.Commons.Exceptions;
using ChatTogether.Dal.Dbos;
using ChatTogether.FluentValidator.Validators.Security;
using ChatTogether.Logic.Interfaces;
using ChatTogether.Logic.Interfaces.Security;
using ChatTogether.Ports.Dtos.Security;
using ChatTogether.ViewModels;
using ChatTogether.ViewModels.Security;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Security.Principal;
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

        public SecurityController(
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            ISecurityService securityService,
            IUserService userService,
            LoginModelValidator loginModelValidator,
            RegistraionModelValidator registrationModelValidator
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.securityService = securityService;
            this.userService = userService;
            this.loginModelValidator = loginModelValidator;
            this.registrationModelValidator = registrationModelValidator;
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

                await securityService.ChangeEmail(token, newEmail);
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

                await securityService.ChangePassword(token, newPassword);

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

                await securityService.ChangePasswordRequest(email);

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
                await securityService.ConfirmEmail(email, token);

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

                await securityService.ResendConfirmationEmail(email);

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
                await securityService.ChangeEmailRequest(email);

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
                await securityService.ChangePasswordRequest(email);

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
                (ClaimsPrincipal claimsPrincipal, UserDbo userDbo) = await securityService.SignIn(accountDto);

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
                await securityService.SignUp(accountDto, registrationModel.Nickname);

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

                UserDbo userDbo = await userService.GetUser(nickname);
                UserViewModel userViewModel = mapper.Map<UserViewModel>(userDbo);

                return Ok(userViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //TODO: pomyslec nad refreshem (moze refresh token)
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                IIdentity identity = httpContextAccessor.HttpContext.User.Identity;
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                await httpContextAccessor.HttpContext.SignInAsync(claimsPrincipal);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
