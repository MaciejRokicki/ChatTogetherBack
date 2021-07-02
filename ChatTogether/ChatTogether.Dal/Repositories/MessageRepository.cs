using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Repositories
{
    public class MessageRepository : Repository<Guid, MessageDbo>, IMessageRepository
    {
        private readonly ChatTogetherDbContext chatTogetherDbContext;

        public MessageRepository(ChatTogetherDbContext chatTogetherDbContext) : base(chatTogetherDbContext)
        {
            this.chatTogetherDbContext = chatTogetherDbContext;
        }

        public async Task<IEnumerable<MessageDbo>> GetMessagesAsync(int roomId, int size, DateTime lastMessageDate)
        {
            IEnumerable<MessageDbo> entites = await chatTogetherDbContext
                .Set<MessageDbo>()
                .Include(x => x.User)
                .Where(x => x.RoomId == roomId)
                .Where(x => x.ReceivedTime < lastMessageDate)
                .OrderByDescending(x => x.ReceivedTime)
                .Take(size)
                .Reverse()
                .AsNoTracking()
                .ToListAsync();

            return entites;
        }
    }
}
