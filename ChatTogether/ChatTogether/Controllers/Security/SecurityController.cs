using AutoMapper;
using ChatTogether.FluentValidator.Validators.Security;
using ChatTogether.Logic.Interfaces.Security;
using ChatTogether.Ports.Dtos.Security;
using ChatTogether.ViewModels.Security;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
//TODO: obsługa błędów -> try/catch na kontrolerze, customowe exceptiony w serwisach
//TODO: wystawic endpoint do wylogowywania
namespace ChatTogether.Controllers.Security
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly ISecurityService securityService;
        private readonly LoginModelValidator loginModelValidator;
        private readonly RegistraionModelValidator registrationModelValidator;

        public SecurityController(
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper, 
            ISecurityService securityService, 
            LoginModelValidator loginModelValidator, 
            RegistraionModelValidator registrationModelValidator
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.securityService = securityService;
            this.loginModelValidator = loginModelValidator;
            this.registrationModelValidator = registrationModelValidator;
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> ChangeEmail(string token, string newEmail)
        {
            ValidationResult validationResult = await loginModelValidator.ValidateAsync(new LoginModel() { Email = newEmail,  Password = "strinG1!" });

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await securityService.ChangeEmail(token, newEmail);

            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> ChangePassword(string token, string newPassword)
        {
            ValidationResult validationResult = await loginModelValidator.ValidateAsync(new LoginModel() { Email = "example@example.com", Password = newPassword });

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await securityService.ChangePassword(token, newPassword);

            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            ValidationResult validationResult = await loginModelValidator.ValidateAsync(new LoginModel() { Email = email, Password = "strinG1!" });

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await securityService.SendRequestToChangePassword(email);

            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            ValidationResult validationResult = await loginModelValidator.ValidateAsync(new LoginModel() { Email = email, Password = "strinG1!" });

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await securityService.ConfirmEmail(email, token);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            ValidationResult validationResult = await loginModelValidator.ValidateAsync(new LoginModel() { Email = email, Password = "strinG1!" });

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await securityService.ResendConfirmationEmail(email);

            return Ok();
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> SendRequestToChangeEmail()
        {
            string email = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            await securityService.SendRequestToChangeEmail(email);

            return Ok();
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> SendRequestToChangePassword()
        {
            string email = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            await securityService.SendRequestToChangePassword(email);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignIn([FromBody] LoginModel loginModel)
        {
            ValidationResult validationResult = await loginModelValidator.ValidateAsync(loginModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            AccountDto accountDto = mapper.Map<AccountDto>(loginModel);
            ClaimsPrincipal claimsPrincipal = await securityService.SignIn(accountDto);

            await httpContextAccessor.HttpContext.SignInAsync(claimsPrincipal);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignUp([FromBody] RegistrationModel registrationModel)
        {
            ValidationResult validationResult = await registrationModelValidator.ValidateAsync(registrationModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            AccountDto accountDto = mapper.Map<AccountDto>(registrationModel);
            await securityService.SignUp(accountDto, registrationModel.Nickname);

            return Ok();
        }
    }
}
