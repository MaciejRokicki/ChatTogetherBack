using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Interfaces;
using ChatTogether.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }

        public async Task Add(MessageDbo messageDbo)
        {
            await messageRepository.CreateAsync(messageDbo);
        }

        public async Task<IEnumerable<MessageDbo>> GetMessagesAsync(int roomId, int size, DateTime lastMessageDate)
        {
            IEnumerable<MessageDbo> messages = await messageRepository.GetMessagesAsync(roomId, size, lastMessageDate);

            return messages;
        }
    }
}
