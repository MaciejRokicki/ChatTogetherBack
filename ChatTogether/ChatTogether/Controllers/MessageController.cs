using AutoMapper;
using ChatTogether.Commons.Role;
using ChatTogether.Dal.Dbos;
using ChatTogether.FluentValidator.Validators;
using ChatTogether.Logic.Interfaces.Services;
using ChatTogether.ViewModels;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ChatTogether.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IMessageService messageService;
        private readonly IWebHostEnvironment env;
        private readonly FileToUploadModelValidator fileToUploadModelValidator;

        public MessageController(
            IMapper mapper,
            IMessageService messageService,
            IWebHostEnvironment env,
            FileToUploadModelValidator fileToUploadModelValidator)
        {
            this.mapper = mapper;
            this.messageService = messageService;
            this.env = env;
            this.fileToUploadModelValidator = fileToUploadModelValidator;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetMessages(int roomId, int size, DateTime? lastMessageDate)
        {
            try
            {
                if(lastMessageDate == null)
                {
                    lastMessageDate = DateTime.UtcNow;
                }

                lastMessageDate = lastMessageDate.Value.ToUniversalTime();

                IEnumerable<MessageDbo> paginationPageDbo = await messageService.GetMessagesAsync(roomId, size, lastMessageDate.Value);
                IEnumerable<MessageViewModel> paginationPageViewModel = mapper.Map<IEnumerable<MessageViewModel>>(paginationPageDbo);

                return Ok(paginationPageViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> UploadMessageFiles(IFormCollection formCollection)
        {
            try
            {
                foreach(IFormFile file in formCollection.Files)
                {
                    ValidationResult validationResult = await fileToUploadModelValidator.ValidateAsync(file);

                    if (!validationResult.IsValid)
                    {
                        throw new InvalidDataException(validationResult.ToString());
                    }
                }

                List<MessageFileDbo> filesDbo = await messageService.UploadMessageFilesAsync(formCollection, env.ContentRootPath);

                List<MessageFileViewModel> files = mapper.Map<List<MessageFileViewModel>>(filesDbo);

                return Ok(files);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
