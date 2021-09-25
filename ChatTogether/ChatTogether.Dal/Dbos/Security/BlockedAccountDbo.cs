using ChatTogether.Commons.GenericRepository;
using System;

namespace ChatTogether.Dal.Dbos.Security
{
    public class BlockedAccountDbo : DboModel<int>
    {
        public string Reason { get; set; }
        public DateTime? BlockedTo { get; set; }
        public DateTime Created { get; set; }
        public int CreatedById { get; set; }
        public AccountDbo CreatedBy { get; set; }

        public AccountDbo Account { get; set; }

        public BlockedAccountDbo()
        {
            Created = DateTime.UtcNow;
        }
    }
}
