using ChatTogether.ViewModels;
using System;
using System.Collections.Generic;

namespace ChatTogether.Ports.HubModels
{
    public class MessageHubModel
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public string Nickname { get; set; }
        public int RoomId { get; set; }
        public DateTime SendTime { get; set; }
        public DateTime ReceivedTime { get; set; }

        public ICollection<MessageFileHubModel> Files { get; set; }
    }
}
