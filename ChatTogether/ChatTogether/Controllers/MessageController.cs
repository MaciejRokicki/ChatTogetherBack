using AutoMapper;
using ChatTogether.Dal.Dbos;
using ChatTogether.Logic.Interfaces.Services;
using ChatTogether.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTogether.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IMessageService messageService;

        public MessageController(
            IMapper mapper,
            IMessageService messageService)
        {
            this.mapper = mapper;
            this.messageService = messageService;
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

                IEnumerable<MessageDbo> paginationPageDbo = await messageService.GetMessagesAsync(roomId, size, lastMessageDate.Value);
                IEnumerable<MessageViewModel> paginationPageViewModel = mapper.Map<IEnumerable<MessageViewModel>>(paginationPageDbo);
                return Ok(paginationPageViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
