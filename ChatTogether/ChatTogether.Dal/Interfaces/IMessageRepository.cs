using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTogether.Dal.Interfaces
{
    public interface IMessageRepository : IRepository<Guid, MessageDbo>
    {
        Task<IEnumerable<MessageDbo>> GetMessagesAsync(int roomId, int size, DateTime lastMessageDate);
        Task<bool> DeleteAsync(Guid id);
    }
}
