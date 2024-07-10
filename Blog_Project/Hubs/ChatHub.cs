using Blog_Project.CORE.Models.Domain;
using Blog_Project.EF.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace Blog_Project.Hubs

{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly  SharedDataBase sharedDataBase;

        public UserManager<IdentityUser> UserManager { get; }

        public ChatHub(SharedDataBase sharedDataBase, UserManager<IdentityUser> userManager)
        {
            
            this.sharedDataBase = sharedDataBase;
            UserManager = userManager;
        }

        [Authorize(Roles = "Reader")]
        public override async Task OnConnectedAsync()
        {
            
            await Clients.All.ReceiveMessage("hello there");
        }
        // for testing only
        /*public async Task JoinChat(UserConnection userConnection)
        {
            var userName = Context.User.Identity.Name;
            
            await Clients.All.ReceiveMessage("ReceiveMessage", "admin", $"{userName} has joined");
        }*/

        [Authorize(Roles = "Reader")]
        public async Task JoinChatRoom(UserConnection userConnection)
        {
            

            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.ChatRoom);
            
            sharedDataBase.connections[Context.ConnectionId] = userConnection;

            await Clients.Group(userConnection.ChatRoom)
                .ReceiveMessage("ReceiveMessage", "admin", $"{userConnection.Username} has joined {userConnection.ChatRoom}");
        }

        [Authorize(Roles = "Reader")]
        public async Task SendMessage(string msg)
        {
            
            if (sharedDataBase.connections.TryGetValue(Context.ConnectionId, out UserConnection connection))
            {
                await Clients.Group(connection.ChatRoom)
                    .ReceiveMessage("ReceiveSpecificMessage", connection.Username, msg);
            }
        }
        
    }
}
