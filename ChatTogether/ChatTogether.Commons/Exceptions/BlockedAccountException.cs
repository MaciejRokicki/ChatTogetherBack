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
                { "Blocked", blocked },
                { "BlockedTo", blockedTo }
            });
        }
    }
}
