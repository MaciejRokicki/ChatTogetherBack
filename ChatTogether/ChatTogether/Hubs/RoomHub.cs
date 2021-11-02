using AutoMapper;
using ChatTogether.Dal.Dbos;
using ChatTogether.Hubs.Interfaces;
using ChatTogether.Logic.Interfaces.MemoryStores;
using ChatTogether.Logic.Interfaces.Services;
using ChatTogether.Ports.HubModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatTogether.Hubs
{
    [Authorize]
    public class RoomHub : Hub<IRoomHub>
    {
        private const string _groupRoom = "GROUP_ROOM_";

        private readonly IMapper mapper;
        private readonly IMessageService messageService;
        private readonly IRoomMemoryStore roomMemoryStore;

        public RoomHub(
            IMapper mapper,
            IMessageService messageService,
            IRoomMemoryStore roomMemoryStore
            )
        {
            this.mapper = mapper;
            this.messageService = messageService;
            this.roomMemoryStore = roomMemoryStore;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.GetRooms(roomMemoryStore.GetRooms());
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            roomMemoryStore.Exit(Context.ConnectionId);

            await Clients.All.GetRooms(roomMemoryStore.GetRooms());

            await base.OnDisconnectedAsync(exception);
        }

        public async Task GetRooms()
        {
            await Clients.All.GetRooms(roomMemoryStore.GetRooms());
        }

        public async Task EnterRoom(int roomId)
        {
            int userId = int.Parse(Context.User.FindFirstValue("UserId"));

            UserHubModel userHubModel = new UserHubModel()
            {
                Id = userId,
                ConnectionId = Context.ConnectionId
            };

            bool res = roomMemoryStore.Enter(roomId, userHubModel);

            if(res)
            {
                await Groups.AddToGroupAsync(userHubModel.ConnectionId, _groupRoom + roomId);
                await Clients.All.GetRooms(roomMemoryStore.GetRooms());
            }
        }

        public async Task ExitRoom(int roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, _groupRoom + roomId);
            roomMemoryStore.Exit(roomId, Context.ConnectionId);

            await Clients.All.GetRooms(roomMemoryStore.GetRooms());
        }

        public async Task SendMessage(MessageHubModel messageHubModel)
        {
            int userId = int.Parse(Context.User.FindFirstValue("UserId"));
            string nickname = Context.User.FindFirstValue("Nickname");

            messageHubModel.UserId = userId;
            messageHubModel.Nickname = nickname;
            messageHubModel.ReceivedTime = DateTime.UtcNow;

            await Clients.GroupExcept(_groupRoom + messageHubModel.RoomId, Context.ConnectionId).ReceiveMessage(messageHubModel);

            MessageDbo messageDbo = mapper.Map<MessageDbo>(messageHubModel);
            await messageService.Add(messageDbo);
        }

        public async Task DeleteMessage(int roomId, Guid id)
        {
            bool result = await messageService.Delete(id);

            if(result)
            {
                await Clients.Group(_groupRoom + roomId).DeleteMessage(id);
            }
        }
    }
}
