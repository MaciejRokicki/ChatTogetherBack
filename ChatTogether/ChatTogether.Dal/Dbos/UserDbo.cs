using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos.Security;
using System;

namespace ChatTogether.Dal.Dbos
{
    public class UserDbo : DboModel
    {
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string City { get; set; }
        public string Description { get; set; }

        public int AccountId { get; set; }
        public AccountDbo Account { get; set; }
    }
}
