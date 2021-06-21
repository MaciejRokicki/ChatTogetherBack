using System;

namespace ChatTogether.Commons.Exceptions
{
    public class InvalidDataException : Exception
    {
        public InvalidDataException() : base("Invalid data.")
        {
        }
    }
}
