using System;

namespace ChatTogether.Logic.Interfaces.MemoryStores
{
    public interface IMessageMemoryStore
    {
        void AddMessageToDelete(Guid id);
        bool Contains(Guid id);
        void DeleteMessageToDelete(Guid id);
    }
}
