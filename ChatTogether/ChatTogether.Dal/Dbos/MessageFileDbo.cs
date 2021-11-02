using ChatTogether.Commons.GenericRepository;
using System;

namespace ChatTogether.Dal.Dbos
{
    public class MessageFileDbo : DboModel<int>
    {
        public MessageDbo Message { get; set; }
        public Guid MessageId { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }
        public string SourceName { get; set; }
        public string ThumbnailName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
