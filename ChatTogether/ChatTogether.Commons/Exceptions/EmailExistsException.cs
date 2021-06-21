using System;

namespace ChatTogether.Commons.Exceptions
{
    public class EmailExistsException : Exception
    {
        public EmailExistsException() : base("Email is in use.")
        {
        }
    }
}
