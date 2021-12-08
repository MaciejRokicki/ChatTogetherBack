using AutoMapper;
using ChatTogether.Commons.Exceptions;
using ChatTogether.Commons.Page;
using ChatTogether.Commons.Role;
using ChatTogether.Dal.Dbos;
using ChatTogether.FluentValidation.Validators;
using ChatTogether.Logic.Interfaces.Services;
using ChatTogether.ViewModels;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatTogether.Controllerss
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly UserModelValidator userModelValidator;

        public UserController(
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IUserService userService, 
            UserModelValidator userModelValidator)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.userService = userService;
            this.userModelValidator = userModelValidator;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUser(string nickname)
        {
            try
            {
                ValidationResult validationResult = await userModelValidator.ValidateAsync(new UserViewModel() { Nickname = nickname });

                if (!validationResult.IsValid)
                {
                    throw new InvalidDataException();
                }

                UserDbo userDbo = await userService.GetUserAsync(nickname);
                UserViewModel userViewModel = mapper.Map<UserViewModel>(userDbo);
                return Ok(userViewModel);
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

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUsers(int page = 1, string search = "", Role? role = null)
        {
            try
            {
                Page<UserDbo> pageDbo = await userService.GetUsersAsync(page, 10, search, role);
                Page<UserViewModel> pageViewModel = mapper.Map<Page<UserViewModel>>(pageDbo);

                return Ok(pageViewModel);
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
        [Authorize]
        public async Task<IActionResult> ChangeUserData([FromBody] UserViewModel userViewModel)
        {
            try
            {
                ValidationResult validationResult = await userModelValidator.ValidateAsync(userViewModel);

                if (!validationResult.IsValid)
                {
                    throw new InvalidDataException();
                }

                string email = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

                UserDbo userDbo = mapper.Map<UserDbo>(userViewModel);

                userDbo = await userService.UpdateAsync(email, userDbo);
                userViewModel = mapper.Map<UserViewModel>(userDbo);
                return Ok(userViewModel);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(NicknameExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("[action]")]
        [Authorize]
        public async Task<IActionResult> ChangeNickname([FromBody] string nickname)
        {
            try
            {
                ValidationResult validationResult = await userModelValidator.ValidateAsync(new UserViewModel() { Nickname = nickname });

                if (!validationResult.IsValid)
                {
                    throw new InvalidDataException();
                }

                string email = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

                await userService.ChangeNicknameAsync(email, nickname);
                return Ok();
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
    }
}
