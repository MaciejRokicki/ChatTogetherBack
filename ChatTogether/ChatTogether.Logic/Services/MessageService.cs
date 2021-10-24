using ChatTogether.Commons.ConfigurationModels;
using ChatTogether.Commons.ImageService;
using ChatTogether.Commons.RandomStringGenerator;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Interfaces;
using ChatTogether.Logic.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Services
{
    public class MessageService : IMessageService
    {
        private readonly StaticFilesConfiguration staticFilesConfiguration;
        private readonly IImageService imageService;
        private readonly IMessageRepository messageRepository;
        private readonly IRandomStringGenerator randomStringGenerator;

        public MessageService(
            IOptions<StaticFilesConfiguration> staticFilesConfiguration,
            IImageService imageService,
            IMessageRepository messageRepository,
            IRandomStringGenerator randomStringGenerator)
        {
            this.staticFilesConfiguration = staticFilesConfiguration.Value;
            this.imageService = imageService;
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
                string generatedFileName = randomStringGenerator.Generate(RandomStringType.Path);
                string fullPath = Path.Combine(contentRootPath, staticFilesConfiguration.Path, $"{generatedFileName}.{fileExtension}");
                string sourceName = $"{generatedFileName}.{fileExtension}";

                using (FileStream fs = File.Create(fullPath))
                {
                    await file.CopyToAsync(fs);
                    await fs.FlushAsync();
                }

                MessageFileDbo messageFileDbo = new MessageFileDbo()
                {
                    FileName = fileName,
                    SourceName = sourceName,
                    ThumbnailName = String.Empty,
                    Type = file.ContentType
                };

                if (file.ContentType.Contains("image/") && file.ContentType != "image/gif")
                {
                    Bitmap thumbnail = imageService.CreateThumbnail(file);

                    string thumbnailFileName = randomStringGenerator.Generate(RandomStringType.Path);
                    string thumbnailFullPath = Path.Combine(contentRootPath, staticFilesConfiguration.Path, $"{thumbnailFileName}.{fileExtension}");
                    string thumbnailName = $"{thumbnailFileName}.{fileExtension}";

                    thumbnail.Save(thumbnailFullPath);

                    messageFileDbo.ThumbnailName = thumbnailName;
                }

                files.Add(messageFileDbo);
            }

            return files;
        }
    }
}
