using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Interfaces;

namespace ChatTogether.Dal.Repositories
{
    public class RoomRepository : Repository<RoomDbo>, IRoomRepository
    {
        private readonly ChatTogetherDbContext chatTogetherDbContext;

        public RoomRepository(ChatTogetherDbContext chatTogetherDbContext) : base (chatTogetherDbContext)
        {
            this.chatTogetherDbContext = chatTogetherDbContext;
        }
    }
}
