using ChatTogether.Dal.Dbos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces.Services
{
    public interface IMessageService
    {
        void Create(MessageDbo messageDbo);
        Task<IEnumerable<MessageDbo>> GetMessagesAsync(int roomId, int size, DateTime lastMessageDate);
        Task<List<MessageFileDbo>> UploadMessageFilesAsync(IFormCollection formCollection, string contentRootPath);
        Task DeleteAsync(Guid id);
    }
}
