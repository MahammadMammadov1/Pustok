using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Pustok.Core.Models;

namespace SignalRChat.Hubs
{
	public class ChatHub : Hub
	{
        private readonly UserManager<AppUser> _userManager;

        public ChatHub(UserManager<AppUser> userManager)
        {
            this._userManager = userManager;
        }
        public async Task SendMessage(string user, string message)
		{
			await Clients.All.SendAsync("ReceiveMessage", user, message);
		}

        public override async Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var user =await _userManager.FindByNameAsync(Context.User.Identity.Name);
                if (user != null)
                {
                    user.CoonectionId = Context.ConnectionId;
                    await _userManager.UpdateAsync(user);
                    await Clients.All.SendAsync("OnConnect", user.Id);
                }
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if(Context.User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
                if (user != null)
                {
                    user.CoonectionId = null;
                    await _userManager.UpdateAsync(user);
                    await Clients.All.SendAsync("DisConnect", user.Id);
                }
            }
        }
    }
}