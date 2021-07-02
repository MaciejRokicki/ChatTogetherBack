using ChatTogether.HubModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTogether.Hubs
{
    public interface IRoomHub
    {
        Task GetRooms(ICollection<RoomHubModel> rooms);
        Task ReceiveMessage(MessageHubModel messageHubModel);
    }
}
