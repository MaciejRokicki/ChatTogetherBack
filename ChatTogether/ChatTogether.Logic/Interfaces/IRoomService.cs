using ChatTogether.Dal.Dbos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces
{
    public interface IRoomService
    {
        Task<RoomDbo> GetRoom(int id);
        Task<IEnumerable<RoomDbo>> GetRooms();
    }
}
