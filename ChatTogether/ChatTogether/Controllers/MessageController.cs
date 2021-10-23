using AutoMapper;
using ChatTogether.Dal.Dbos;
using ChatTogether.Hubs;
using ChatTogether.Hubs.Interfaces;
using ChatTogether.Logic.Interfaces.Services;
using ChatTogether.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
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

        public MessageController(
            IMapper mapper,
            IMessageService messageService,
            IWebHostEnvironment env)
        {
            this.mapper = mapper;
            this.messageService = messageService;
            this.env = env;
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
                List<MessageFileDbo> filesDbo = await messageService.UploadMessageFiles(formCollection, env.ContentRootPath);

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
