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

        public async Task CreateRoomAsync(RoomDbo roomDbo)
        {
            await roomRepository.CreateAsync(roomDbo);
        }

        public async Task DeleteRoomAsync(int id)
        {
            await roomRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<RoomDbo> GetRoomAsync(int id)
        {
            RoomDbo roomDbo = await roomRepository.GetAsync(x => x.Id == id);

            return roomDbo;
        }

        public async Task<IEnumerable<RoomDbo>> GetRoomsAsync()
        {
            IEnumerable<RoomDbo> paginationPage = await roomRepository.GetManyAsync();

            return paginationPage;
        }

        public async Task UpdateRoomAsync(RoomDbo roomDbo)
        {
            await roomRepository.UpdateAsync(roomDbo);
        }
    }
}
