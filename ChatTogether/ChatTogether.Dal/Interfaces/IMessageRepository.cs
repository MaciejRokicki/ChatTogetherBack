using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos;
using System;

namespace ChatTogether.Dal.Interfaces
{
    public interface IMessageRepository : IRepository<Guid, MessageDbo>
    {
    }
}
