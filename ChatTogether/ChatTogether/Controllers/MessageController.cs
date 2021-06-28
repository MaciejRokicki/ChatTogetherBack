using AutoMapper;
using ChatTogether.Commons.Pagination.Models;
using ChatTogether.Dal.Dbos;
using ChatTogether.Logic.Interfaces;
using ChatTogether.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<IActionResult> GetMessages(int roomId, int pageSize, DateTime lastMessageDate)
        {
            try
            {
                PaginationPage<MessageDbo> paginationPageDbo = await messageService.GetMessagePage(roomId, pageSize, lastMessageDate);
                PaginationPage<MessageViewModel> paginationPageViewModel = mapper.Map<PaginationPage<MessageViewModel>>(paginationPageDbo);
                return Ok(paginationPageViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
