using System;

namespace ChatTogether.ViewModels.Security
{
    public class BlockAccountModel
    {
        public int UserId { get; set; }
        public string Reason { get; set; }
        public DateTime? BlockedTo { get; set; } = null;
    }
}
