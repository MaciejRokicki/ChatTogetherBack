using ChatTogether.Commons.Pagination.Models;
using ChatTogether.Dal.Dbos;
using System;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces
{
    public interface IMessageService
    {
        Task Add(MessageDbo messageDbo);
        Task<PaginationPage<MessageDbo>> GetMessagePage(int roomId, int pageSize, DateTime lastMessageDate);
    }
}
