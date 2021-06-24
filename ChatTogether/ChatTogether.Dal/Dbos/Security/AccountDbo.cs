using ChatTogether.Commons.GenericRepository;
using System;

namespace ChatTogether.Dal.Dbos.Security
{
    public class AccountDbo : DboModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsConfirmed { get; set; }

        public UserDbo User { get; set; }
        public ConfirmEmailTokenDbo ConfirmEmailTokenDbo { get; set; }
        public ChangeEmailTokenDbo ChangeEmailTokenDbo { get; set; }
        public ChangePasswordTokenDbo ChangePasswordTokenDbo { get; set; }
    }
}
