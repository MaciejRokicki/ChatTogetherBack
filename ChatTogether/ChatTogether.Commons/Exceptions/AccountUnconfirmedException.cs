using System;

namespace ChatTogether.Commons.Exceptions
{
    public class AccountUnconfirmedException : Exception
    {
        public AccountUnconfirmedException() : base("Unconfirmed account.")
        {
        }
    }
}
