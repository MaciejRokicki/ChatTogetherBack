using ChatTogether.Commons.GenericRepository;

namespace ChatTogether.Dal.Dbos
{
    public class RoomDbo : DboModel
    {
        public string Name { get; set; }
        public int MaxPeople { get; set; }
    }
}
