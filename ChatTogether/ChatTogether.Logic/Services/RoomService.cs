using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Interfaces;
using ChatTogether.Logic.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }

        public async Task<RoomDbo> GetRoom(int id)
        {
            RoomDbo roomDbo = await roomRepository.GetAsync(x => x.Id == id);

            return roomDbo;
        }

        public async Task<IEnumerable<RoomDbo>> GetRooms()
        {
            IEnumerable<RoomDbo> paginationPage = await roomRepository.GetManyAsync();

            return paginationPage;
        }
    }
}
