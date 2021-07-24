using System.Collections.Generic;

namespace ChatTogether.Ports.HubModels
{
    public class RoomHubModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrentPeople { get; set; } = 0;
        public int MaxPeople { get; set; }

        public List<UserHubModel> Users { get; set; }
    }
}
