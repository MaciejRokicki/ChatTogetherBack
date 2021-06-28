using System;

namespace ChatTogether.ViewModels
{
    public class MessageViewModel
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public string Nickname { get; set; }
        public int RoomId { get; set; }
        public DateTime SendTime { get; set; }
        public DateTime ReceivedTime { get; set; }
    }
}
