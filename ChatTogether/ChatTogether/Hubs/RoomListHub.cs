using ChatTogether.Logic.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChatTogether.Hubs
{
    public class RoomListHub : Hub
    {
        //private readonly IExampleService exampleService;

        //public RoomListHub(IExampleService exampleService)
        //{
        //    this.exampleService = exampleService;
        //}

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public Task ExampleMethod(string txt)
        {
            throw new NotImplementedException();
        }
    }
}
