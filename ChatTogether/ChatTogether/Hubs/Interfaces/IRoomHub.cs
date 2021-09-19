using ChatTogether.Ports.HubModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTogether.Hubs.Interfaces
{
    public interface IRoomHub
    {
        Task GetRooms(ICollection<RoomHubModel> rooms);
        Task ReceiveMessage(MessageHubModel messageHubModel);
        Task RemoveRoomUsers();
    }
}
