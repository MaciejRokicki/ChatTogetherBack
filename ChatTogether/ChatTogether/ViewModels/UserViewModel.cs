using System;

namespace ChatTogether.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public DateTime? BirthDate { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public bool IsBlocked { get; set; }
    }
}
