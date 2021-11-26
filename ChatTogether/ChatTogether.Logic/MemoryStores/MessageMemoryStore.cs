using ChatTogether.Logic.Interfaces.MemoryStores;
using System;
using System.Collections.Generic;

namespace ChatTogether.Logic.MemoryStores
{
    public class MessageMemoryStore : IMessageMemoryStore
    {
        private List<Guid> messagesToDelete;

        public MessageMemoryStore()
        {
            messagesToDelete = new List<Guid>();
        }

        public void AddMessageToDelete(Guid id)
        {
            messagesToDelete.Add(id);
        }

        public bool Contains(Guid id)
        {
            return messagesToDelete.Contains(id);
        }

        public void DeleteMessageToDelete(Guid id)
        {
            messagesToDelete.Remove(id);
        }
    }
}
