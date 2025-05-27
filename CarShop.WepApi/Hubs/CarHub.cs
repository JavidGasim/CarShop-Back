using Microsoft.AspNetCore.SignalR;

namespace CarShop.WepApi.Hubs
{
    public class CarHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var username = Context.User?.Identity?.Name;
            Console.WriteLine($"Username: {username} Line10");
            if (!string.IsNullOrEmpty(username))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, username);
                Console.WriteLine($"User {username} added to group. Line14");

            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = Context.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(username))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, username);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
