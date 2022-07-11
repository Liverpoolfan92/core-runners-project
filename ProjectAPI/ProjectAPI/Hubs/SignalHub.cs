using Microsoft.AspNetCore.SignalR;
using ProjectAPI.Data.Models;

namespace ProjectAPI.Hubs
{
    public class SignalHub : Hub
    {
        public void BroadcastSeat(Seat wp)
        {
            Clients.All.SendAsync("ReceiveSeat", wp);
        }

        //tva trqbva da razbera za kakvo se polzva
        public void BroadcastMessage(string message)
        {
            Clients.All.SendAsync("ReceiveMessage", message);
        }

        //...
    }
}
