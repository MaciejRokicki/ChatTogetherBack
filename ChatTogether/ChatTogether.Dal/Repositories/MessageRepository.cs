using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
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

        public MessageDbo Create(MessageDbo messageDbo)
        {
            chatTogetherDbContext
                .Set<MessageDbo>()
                .Add(messageDbo);

            chatTogetherDbContext.SaveChanges();

            return messageDbo;
        }

        public override async Task<MessageDbo> GetAsync(Expression<Func<MessageDbo, bool>> exp)
        {
            MessageDbo messageDbo = await chatTogetherDbContext
                .Set<MessageDbo>()
                .Include(x => x.Files)
                .FirstOrDefaultAsync(exp);

            return messageDbo;
        }

        public async Task<IEnumerable<MessageDbo>> GetMessagesAsync(int roomId, int size, DateTime lastMessageDate)
        {
            IEnumerable<MessageDbo> entites = await chatTogetherDbContext
                .Set<MessageDbo>()
                .Include(x => x.User)
                .Include(x => x.Files)
                .Where(x => x.RoomId == roomId)
                .Where(x => x.ReceivedTime < lastMessageDate)
                .OrderByDescending(x => x.ReceivedTime)
                .Take(size)
                .Reverse()
                .AsNoTracking()
                .ToListAsync();

            return entites;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            MessageDbo messageDbo = await GetAsync(x => x.Id == id);

            if(messageDbo == null)
            {
                return false;
            }

            chatTogetherDbContext
                .Set<MessageDbo>()
                .Remove(messageDbo);

            await chatTogetherDbContext.SaveChangesAsync();

            return true;
        }
    }
}
