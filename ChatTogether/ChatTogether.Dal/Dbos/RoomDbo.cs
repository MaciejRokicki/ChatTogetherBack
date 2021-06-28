using ChatTogether.Commons.GenericRepository;
using System.Collections.Generic;

namespace ChatTogether.Dal.Dbos
{
    public class RoomDbo : DboModel<int>
    {
        public string Name { get; set; }
        public int MaxPeople { get; set; }

        public ICollection<MessageDbo> Messages { get; set; }
    }
}
