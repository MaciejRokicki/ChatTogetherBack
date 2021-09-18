using ChatTogether.Ports.HubModels;
using System.Collections.Generic;

namespace ChatTogether.Logic.Interfaces.MemoryStores
{
    public interface IUserMemoryStore
    {
        UserHubModel GetUser(int userId);
        ICollection<UserHubModel> GetUsers();
        void Enter(UserHubModel userHubModel);
        void Exit(UserHubModel userHubModel);
    }
}
