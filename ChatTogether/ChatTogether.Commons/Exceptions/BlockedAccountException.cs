using System;
using System.Collections.Generic;

namespace ChatTogether.Commons.Exceptions
{
    public class BlockedAccountException : Exception
    {
        public BlockedAccountException(string reason, DateTime blocked, DateTime? blockedTo = null) : base("Blocked account.")
        {
            Data.Add("BlockedAccount", new Dictionary<string, object>()
            {
                { "Reason", reason },
                { "Blocked", DateTime.SpecifyKind(blocked, DateTimeKind.Utc) },
                { "BlockedTo", blockedTo.HasValue ? DateTime.SpecifyKind(blockedTo.Value, DateTimeKind.Utc) : blockedTo }
            });
        }
    }
}
