using ChatTogether.Dal.Dbos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces.Services
{
    public interface IMessageService
    {
        Task Add(MessageDbo messageDbo);
        Task<IEnumerable<MessageDbo>> GetMessagesAsync(int roomId, int size, DateTime lastMessageDate);
    }
}
