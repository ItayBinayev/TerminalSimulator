using System.ComponentModel.DataAnnotations;

namespace TerminalProjectApi.Models
{
    public class HistoryFlight
    {
        public int FlightNumber { get; set; }
        public bool IsArrival { get; set; }
        public string? AirLine { get; set; }
        public DateTime MadeContact { get; set; } 
        public DateTime FlightFinished { get; set; } = DateTime.Now;

        public override string ToString()
        {
            var status = IsArrival ? "Departing" : "Arriving";
            return $"flight number: {FlightNumber}, status:{status}, Airline : {AirLine}, registered at airport at:{MadeContact:G} ,left the airport at:{FlightFinished:G} ";
        }
    }
}
