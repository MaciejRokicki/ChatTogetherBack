using System;

namespace ChatTogether.ViewModels.Security
{
    public class BlockedAccountViewModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Reason { get; set; }
        public DateTime? BlockedTo { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByEmail { get; set; }
        public string CreatedByNickname { get; set; }
    }
}
