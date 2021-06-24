using System;

namespace ChatTogether.Commons.Exceptions
{
    public class NicknameExistsException : Exception
    {
        public NicknameExistsException() : base("Nickname is in use.")
        {
        }
    }
}
