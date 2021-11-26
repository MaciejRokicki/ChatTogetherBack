using AutoMapper;
using ChatTogether.Dal.Dbos;
using ChatTogether.Logic.Interfaces.MemoryStores;
using ChatTogether.Logic.Interfaces.Services;
using ChatTogether.Ports.HubModels;
using SimpleInjector;
using System.Collections.Generic;
using System.Linq;

namespace ChatTogether.Logic.MemoryStores
{
    public class RoomMemoryStore : IRoomMemoryStore
    {
        private readonly IMapper mapper;
        private readonly IRoomService roomService;

        public List<RoomHubModel> Rooms;

        public RoomMemoryStore(Container container)
        {
            mapper = container.GetInstance<IMapper>();
            roomService = container.GetInstance<IRoomService>();

            IEnumerable<RoomDbo> paginationPageDbo = roomService.GetRoomsAsync().Result;
            IEnumerable<RoomHubModel> paginationPageViewModel = mapper.Map<IEnumerable<RoomHubModel>>(paginationPageDbo);
            Rooms = paginationPageViewModel.ToList();

            foreach (RoomHubModel room in Rooms)
            {
                room.Users = new List<UserHubModel>();
            }
        }

        public RoomHubModel GetRoom(int roomId) => Rooms.FirstOrDefault(x => x.Id == roomId);
        public ICollection<RoomHubModel> GetRooms() => Rooms;

        public bool Enter(int roomId, UserHubModel userHubModel)
        {
            RoomHubModel room = Rooms.FirstOrDefault(x => x.Id == roomId);

            if(room.CurrentPeople >= room.MaxPeople)
            {
                return false;
            }

            room.CurrentPeople++;
            room.Users.Add(userHubModel);

            return true;
        }

        public void Exit(string connectionId)
        {
            foreach(RoomHubModel room in Rooms)
            {
                UserHubModel userHubModel = room.Users.FirstOrDefault(x => x.ConnectionId == connectionId);

                if(userHubModel != null)
                {
                    room.CurrentPeople--;
                    room.Users.Remove(userHubModel);
                }
            }
        }

        public void Exit(int roomId, string connectionId)
        {
            RoomHubModel room = Rooms.FirstOrDefault(x => x.Id == roomId);

            if(room != null)
            {
                UserHubModel userHubModel = room.Users.FirstOrDefault(x => x.ConnectionId == connectionId);

                if (userHubModel != null)
                {
                    room.Users.Remove(userHubModel);
                    room.CurrentPeople--;
                }
            }
        }

        public void CreateRoom(RoomDbo roomDbo)
        {
            RoomHubModel roomHubModel = mapper.Map<RoomHubModel>(roomDbo);
            roomHubModel.Users = new List<UserHubModel>();

            Rooms.Add(roomHubModel);
        }

        public RoomHubModel UpdateRoom(RoomDbo roomDbo)
        {
            RoomHubModel roomHubModel = mapper.Map<RoomHubModel>(roomDbo);
            roomHubModel.Users = new List<UserHubModel>();

            RoomHubModel room = Rooms.First(x => x.Id == roomHubModel.Id);

            int id = Rooms.IndexOf(room);
            Rooms[id] = roomHubModel;

            return room;
        }

        public bool DeleteRoom(int id)
        {
            RoomHubModel room = Rooms.First(x => x.Id == id);

            Rooms.RemoveAt(Rooms.IndexOf(room));

            return true;
        }
    }
}
