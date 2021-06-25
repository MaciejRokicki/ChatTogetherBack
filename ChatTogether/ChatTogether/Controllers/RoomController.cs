using AutoMapper;
using ChatTogether.Commons.Pagination.Models;
using ChatTogether.Dal.Dbos;
using ChatTogether.Logic.Interfaces;
using ChatTogether.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ChatTogether.Controllerss
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRoomService roomService;

        public RoomController(
            IMapper mapper,
            IRoomService roomService)
        {
            this.mapper = mapper;
            this.roomService = roomService;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetRoom(int id)
        {
            try
            {
                RoomDbo roomDbo = await roomService.GetRoom(id);
                RoomViewModel roomViewModel = mapper.Map<RoomViewModel>(roomDbo);
                return Ok(roomViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetRooms()
        {
            try
            {
                PaginationPage<RoomDbo> paginationPageDbo = await roomService.GetRooms();
                PaginationPage<RoomViewModel> paginationPageViewModel = mapper.Map<PaginationPage<RoomViewModel>>(paginationPageDbo);
                return Ok(paginationPageViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
