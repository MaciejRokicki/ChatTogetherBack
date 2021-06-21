using System;

namespace ChatTogether.Commons.Exceptions
{
    public class IncorrectDataException : Exception
    {
        public IncorrectDataException() : base("Incorrect data.")
        {
        }
    }
}
