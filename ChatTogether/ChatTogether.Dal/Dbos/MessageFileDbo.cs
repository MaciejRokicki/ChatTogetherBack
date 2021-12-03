using ChatTogether.Commons.GenericRepository;
using System;

namespace ChatTogether.Dal.Dbos
{
    public class MessageFileDbo : DboModel<int>
    {
        public Guid MessageId { get; set; }
        public MessageDbo Message { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }
        public string SourceName { get; set; }
        public string ThumbnailName { get; set; }
    }
}
