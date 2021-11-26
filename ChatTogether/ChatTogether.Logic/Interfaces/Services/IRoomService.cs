using ChatTogether.Dal.Dbos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces.Services
{
    public interface IRoomService
    {
        Task<RoomDbo> GetRoomAsync(int id);
        Task<IEnumerable<RoomDbo>> GetRoomsAsync();
        Task CreateRoomAsync(RoomDbo roomDbo);
        Task UpdateRoomAsync(RoomDbo roomDbo);
        Task DeleteRoomAsync(int id);
    }
}
