using System;

namespace ChatTogether.Commons.Exceptions
{
    public class NotEmptyRoomException : Exception
    {
        public NotEmptyRoomException() : base ("Not empty room exception.")
        {
        }
    }
}
