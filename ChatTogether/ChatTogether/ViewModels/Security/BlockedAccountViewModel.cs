using System;

namespace ChatTogether.ViewModels.Security
{
    public class BlockedAccountViewModel
    {
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Reason { get; set; }
        public DateTime? BlockedTo { get; set; }
        public DateTime Created { get; set; }
    }
}
