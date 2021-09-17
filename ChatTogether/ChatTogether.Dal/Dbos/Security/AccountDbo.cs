using ChatTogether.Commons.GenericRepository;
using ChatTogether.Commons.Role;
using System;

namespace ChatTogether.Dal.Dbos.Security
{
    public class AccountDbo : DboModel<int>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Created { get; set; }
        public bool IsConfirmed { get; set; }
        public Role Role { get; set; }
        public int? BlockedAccountId { get; set; }
        public BlockedAccountDbo BlockedAccountDbo { get; set; }

        public UserDbo User { get; set; }
        public ConfirmEmailTokenDbo ConfirmEmailTokenDbo { get; set; }
        public ChangeEmailTokenDbo ChangeEmailTokenDbo { get; set; }
        public ChangePasswordTokenDbo ChangePasswordTokenDbo { get; set; }

        public AccountDbo()
        {
            Created = DateTime.UtcNow;
        }
    }
}
