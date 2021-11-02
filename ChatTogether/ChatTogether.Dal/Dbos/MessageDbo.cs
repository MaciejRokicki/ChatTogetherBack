using ChatTogether.Commons.GenericRepository;
using System;
using System.Collections.Generic;

namespace ChatTogether.Dal.Dbos
{
    public class MessageDbo : DboModel<Guid>
    {
        public string Message { get; set; }
        public int UserId { get; set; }
        public UserDbo User { get; set; }
        public int RoomId { get; set; }
        public RoomDbo Room { get; set; }
        public DateTime SendTime { get; set; }
        public DateTime ReceivedTime { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<MessageFileDbo> Files { get; set; }
    }
}
