using ChatTogether.Logic.Interfaces.MemoryStores;
using ChatTogether.Ports.HubModels;
using System.Collections.Generic;
using System.Linq;

namespace ChatTogether.Logic.MemoryStores
{
    public class UserMemoryStore : IUserMemoryStore
    {
        public List<UserHubModel> Users;

        public UserMemoryStore()
        {
            Users = new List<UserHubModel>();
        }

        public void Enter(UserHubModel userHubModel)
        {
            Users.Add(userHubModel);
        }

        public void Exit(UserHubModel userHubModel)
        {
            UserHubModel user = Users.FirstOrDefault(x => x.ConnectionId == userHubModel.ConnectionId);

            if(user != null)
            {
                Users.Remove(user);
            }
        }

        public ICollection<UserHubModel> GetUsers()
        {
            return Users;
        }

        public UserHubModel GetUser(int userId)
        {
            return Users.FirstOrDefault(x => x.Id == userId);
        }
    }
}
