using System;

namespace ChatTogether.ViewModels
{
    public class MessageFileHubModel
    {
        public int Id { get; set; }
        public Guid MessageId { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }
        public string SourceName { get; set; }
        public string ThumbnailName { get; set; }
    }
}
