﻿using ChatTogether.Dal.Dbos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces.Services
{
    public interface IRoomService
    {
        Task<RoomDbo> GetRoom(int id);
        Task<IEnumerable<RoomDbo>> GetRooms();
        Task CreateRoom(RoomDbo roomDbo);
        Task UpdateRoom(RoomDbo roomDbo);
        Task DeleteRoom(int id);
    }
}
