using ChatTogether.Commons.Pagination.Models;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Interfaces;
using ChatTogether.Logic.Interfaces;
using System;
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

        public async Task<PaginationPage<MessageDbo>> GetMessagePage(int roomId, int pageSize, DateTime lastMessageDate)
        {
            PaginationQuery paginationQuery = new PaginationQuery()
            {
                Page = 1,
                PageSize = pageSize,
                Sortings = new Sorting[]
                {
                    new Sorting()
                    {
                        FieldName = "SendTime",
                        Ascending = true
                    }
                },
                Filters = new Filter[]
                {
                    new Filter()
                    {
                        FieldName = "SendTime",
                        Operation = Operation.LE,
                        Type = FieldType.STRING_TYPE,
                        Value = lastMessageDate.ToString()
                    }
                }
            };

            PaginationPage<MessageDbo> page = await messageRepository.GetPageAsync(paginationQuery);

            return page;
        }
    }
}
