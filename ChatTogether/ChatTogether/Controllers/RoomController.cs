using AutoMapper;
using ChatTogether.Commons.Role;
using ChatTogether.Dal.Dbos;
using ChatTogether.Logic.Interfaces.Services;
using ChatTogether.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
                IEnumerable<RoomDbo> paginationPageDbo = await roomService.GetRooms();
                IEnumerable<RoomViewModel> paginationPageViewModel = mapper.Map<IEnumerable<RoomViewModel>>(paginationPageDbo);
                return Ok(paginationPageViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        [AuthorizeRoles(Role.ADMINISTRATOR)]
        public async Task<IActionResult> CreateRoom([FromBody] RoomViewModel roomViewModel)
        {
            try
            {
                RoomDbo roomDbo = mapper.Map<RoomDbo>(roomViewModel);
                await roomService.CreateRoom(roomDbo);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        [AuthorizeRoles(Role.ADMINISTRATOR)]
        public async Task<IActionResult> UpdateRoom([FromBody] RoomViewModel roomViewModel)
        {
            try
            {
                RoomDbo roomDbo = mapper.Map<RoomDbo>(roomViewModel);
                await roomService.UpdateRoom(roomDbo);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("[action]")]
        [AuthorizeRoles(Role.ADMINISTRATOR)]
        public async Task<IActionResult> DeleteRoom([FromBody] int roomId)
        {
            try
            {
                await roomService.DeleteRoom(roomId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
