using ChatTogether.Commons.Pagination.Models;
using ChatTogether.Dal.Dbos;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces
{
    public interface IRoomService
    {
        Task<RoomDbo> GetRoom(int id);
        Task<PaginationPage<RoomDbo>> GetRooms();
    }
}
