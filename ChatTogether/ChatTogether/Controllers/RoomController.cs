using AutoMapper;
using ChatTogether.Commons.Role;
using ChatTogether.Dal.Dbos;
using ChatTogether.Hubs;
using ChatTogether.Hubs.Interfaces;
using ChatTogether.Logic.Interfaces.MemoryStores;
using ChatTogether.Logic.Interfaces.Services;
using ChatTogether.Ports.HubModels;
using ChatTogether.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IRoomMemoryStore roomMemoryStore;
        private readonly IHubContext<RoomHub, IRoomHub> roomHub;

        public RoomController(
            IMapper mapper,
            IRoomService roomService,
            IRoomMemoryStore roomMemoryStore,
            IHubContext<RoomHub, IRoomHub> roomHub)
        {
            this.mapper = mapper;
            this.roomService = roomService;
            this.roomMemoryStore = roomMemoryStore;
            this.roomHub = roomHub;
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
                roomMemoryStore.CreateRoom(roomDbo);

                await roomHub.Clients.All.GetRooms(roomMemoryStore.GetRooms());

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
                RoomHubModel roomHubModel = roomMemoryStore.UpdateRoom(roomDbo);

                if (roomHubModel == null)
                {
                    return BadRequest();
                }

                await roomService.UpdateRoom(roomDbo);

                await roomHub.Clients.All.GetRooms(roomMemoryStore.GetRooms());

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
                bool success = roomMemoryStore.DeleteRoom(roomId);

                if (!success)
                {
                    return BadRequest();
                }

                await roomService.DeleteRoom(roomId);

                await roomHub.Clients.All.GetRooms(roomMemoryStore.GetRooms());

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
