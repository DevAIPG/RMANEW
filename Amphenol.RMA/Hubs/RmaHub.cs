using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class RmaHub : Hub
    {
        static int contador = 0;
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
           
        }
        public async Task SendUsuarios(string user)
        {
            await Clients.All.SendAsync("ReceiveMessageUsuarios", user, contador);
        }
        public override async Task OnConnectedAsync()
        {
            contador++;
            await Clients.All.SendAsync("mostrar", contador);
            Console.WriteLine("user connected en total hay " + contador);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            contador--;
            await Clients.All.SendAsync("disconnected", contador);
            await Clients.All.SendAsync("mostrar", contador);
            Console.WriteLine("user disconnected quedan conectados " + contador);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
