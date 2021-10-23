using ChatTogether.Commons.RandomStringGenerator;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Interfaces;
using ChatTogether.Logic.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository messageRepository;
        private readonly IRandomStringGenerator randomStringGenerator;

        public MessageService(
            IMessageRepository messageRepository,
            IRandomStringGenerator randomStringGenerator)
        {
            this.messageRepository = messageRepository;
            this.randomStringGenerator = randomStringGenerator;
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

        public async Task<List<MessageFileDbo>> UploadMessageFiles(IFormCollection formCollection, string contentRootPath)
        {
            List<MessageFileDbo> files = new List<MessageFileDbo>();

            foreach (IFormFile file in formCollection.Files)
            {
                string fileName = file.FileName;
                string fileExtension = file.FileName.Split('.').Last();
                string generatedFileName = randomStringGenerator.Generate();
                string fullPath = Path.Combine(contentRootPath, "static", $"{generatedFileName}.{fileExtension}");
                string sourceName = $"{generatedFileName}.{fileExtension}";

                using (FileStream fs = File.Create(fullPath))
                {
                    await file.CopyToAsync(fs);
                    await fs.FlushAsync();
                }

                files.Add(new MessageFileDbo()
                {
                    FileName = fileName,
                    SourceName = sourceName,
                    ThumbnailName = String.Empty,
                    Type = file.ContentType
                });
            }

            return files;
        }
    }
}
