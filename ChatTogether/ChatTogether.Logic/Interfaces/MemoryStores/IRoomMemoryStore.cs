using ChatTogether.Ports.HubModels;
using System.Collections.Generic;

namespace ChatTogether.Logic.Interfaces.MemoryStores
{
    public interface IRoomMemoryStore
    {
        RoomHubModel GetRoom(int roomId);
        ICollection<RoomHubModel> GetRooms();
        bool Enter(int roomId, UserHubModel userHubModel);
        void Exit(string connectionId);
        void Exit(int roomId, string connectionId);
    }
}
