using Microsoft.AspNetCore.SignalR;
using TerminalProjectApi.Models;

namespace TerminalProjectApi.Hubs
{
    public class TerminalHub: Hub
    {
        
        public async Task LegChanged(int flight , int legId)
        {
            await Clients.All.SendAsync("LegChanged" , flight, legId);
        }
        public async Task FlightDismissed(int flightId, string message)
        {
            await Clients.All.SendAsync("FlightDismissed", flightId);
        }
        public async Task FlightCreated(Flight flight, string message)
        {
            await Clients.All.SendAsync("FlightCreated", flight, message);
        }
    }
}
