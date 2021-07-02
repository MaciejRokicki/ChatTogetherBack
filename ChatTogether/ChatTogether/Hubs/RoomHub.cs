using AutoMapper;
using ChatTogether.Dal.Dbos;
using ChatTogether.HubModels;
using ChatTogether.Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatTogether.Hubs
{
    [Authorize]
    public class RoomHub : Hub<IRoomHub>
    {
        private const string _groupRoom = "GROUP_ROOM_";

        private List<RoomHubModel> Rooms;

        private readonly IMapper mapper;
        private readonly IMessageService messageService;

        public RoomHub(
            IMapper mapper,
            IRoomService roomService,
            IMessageService messageService
            )
        {
            this.mapper = mapper;
            this.messageService = messageService;

            IEnumerable<RoomDbo> paginationPageDbo = roomService.GetRooms().Result;
            IEnumerable<RoomHubModel> paginationPageViewModel = mapper.Map<IEnumerable<RoomHubModel>>(paginationPageDbo);
            Rooms = paginationPageViewModel.ToList();

            foreach(RoomHubModel room in Rooms)
            {
                room.Users = new List<UserHubModel>();
            }
        }

        //TODO: przetestowac po sklejeniu backendu z frontem
        //w przypadku nieoczekiwanego rozlaczenia
        //public override Task OnDisconnectedAsync(Exception exception)
        //{
        //    int userId = int.Parse(Context.User.FindFirstValue("UserId"));
        //    RoomHubModel roomHubModel = Rooms
        //        .Where(x => x.Users
        //            .Where(y => y.Id == userId)
        //            .Any())
        //        .FirstOrDefault();

        //    if(roomHubModel != null)
        //    {
        //        roomHubModel.CurrentPeople--;
        //        UserHubModel userHubModel = roomHubModel.Users
        //            .Where(x => x.Id == userId)
        //            .FirstOrDefault();

        //        roomHubModel.Users.Remove(userHubModel);
        //    }

        //    return base.OnDisconnectedAsync(exception);
        //}

        //po wejsciu w strone z lista pokoi do wyboru
        public async Task GetRooms()
        {
            await Clients.All.GetRooms(Rooms);
        }

        //po wejsciu w dany pokoj
        public async Task EnterRoom(int roomId)
        {
            //RoomHubModel room = Rooms
            //    .Where(x => x.Id == roomId)
            //    .FirstOrDefault();
            //room.CurrentPeople++;

            int userId = int.Parse(Context.User.FindFirstValue("UserId"));
            UserHubModel userHubModel = new UserHubModel()
            {
                Id = userId,
                ConnectionId = Context.ConnectionId
            };
            //room.Users.Add(userHubModel);

            await Groups.AddToGroupAsync(userHubModel.ConnectionId, _groupRoom + roomId);
            await Clients.All.GetRooms(Rooms);
        }

        //po wyjsciu z danego pokoju
        public async Task ExitRoom(int roomId)
        {
            //RoomHubModel room = Rooms
            //    .Where(x => x.Id == roomId)
            //    .FirstOrDefault();

            //room.CurrentPeople--;

            //int userId = int.Parse(Context.User.FindFirstValue("UserId"));
            //UserHubModel userHubModel = room.Users
            //    .Where(x => x.Id == userId)
            //    .FirstOrDefault();

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, _groupRoom + roomId);
            //room.Users.Remove(userHubModel);

            await Clients.All.GetRooms(Rooms);
        }

        //TODO: sprawdzic po sklejeniu frontu z backiem
        //wysylanie wiadomosci i ustawianie nasluchu na odbieranie wiadomosci w dla userow nalezacych do danej grupy/pokoju
        public async Task SendMessage(MessageHubModel messageHubModel)
        {
            int userId = int.Parse(Context.User.FindFirstValue("UserId"));
            string nickname = Context.User.FindFirstValue("Nickname");

            messageHubModel.Id = Guid.NewGuid();
            messageHubModel.UserId = userId;
            messageHubModel.Nickname = nickname;
            messageHubModel.ReceivedTime = DateTime.UtcNow;

            //await Clients.Group(_groupRoom + messageHubModel.RoomId).ReceiveMessage(messageHubModel);
            await Clients.GroupExcept(_groupRoom + messageHubModel.RoomId, Context.ConnectionId).ReceiveMessage(messageHubModel);

            MessageDbo messageDbo = mapper.Map<MessageDbo>(messageHubModel);
            await messageService.Add(messageDbo);
        }
    }
}
