using ChatTogether.Hubs.Interfaces;
using ChatTogether.Logic.Interfaces.MemoryStores;
using ChatTogether.Ports.HubModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatTogether.Hubs
{
    public class InformationHub : Hub<IInformationHub>
    {
        private readonly IUserMemoryStore userMemoryStore;

        public InformationHub(IUserMemoryStore userMemoryStore)
        {
            this.userMemoryStore = userMemoryStore;
        }

        public override async Task OnConnectedAsync()
        {
            UserHubModel userHubModel = new UserHubModel()
            {
                Id = int.Parse(Context.User.FindFirstValue("UserId")),
                ConnectionId = Context.ConnectionId
            };

            userMemoryStore.Enter(userHubModel);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            UserHubModel userHubModel = new UserHubModel()
            {
                Id = int.Parse(Context.User.FindFirstValue("UserId")),
                ConnectionId = Context.ConnectionId
            };

            userMemoryStore.Exit(userHubModel);

            await base.OnDisconnectedAsync(exception);
        }

        //[Authorize]
        //public async Task Signout(int userId)
        //{
        //    UserHubModel userHubModel = userMemoryStore.GetUser(userId);

        //    if (userHubModel != null)
        //    {
        //        await Clients.Client(userHubModel.ConnectionId).Signout();
        //    }
        //}
    }
}
