using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Timers;
using TerminalProjectApi.Database;
using TerminalProjectApi.Interfaces;
using Timer = System.Threading.Timer;

namespace TerminalProjectApi.Models
{
    public class Flight
    {
        public int Id { get; set; }
        [Required]
        public int FlightNumber { get; set; }
        public bool IsArrival { get; set; }
        public string? AirLine { get; set; }
        public DateTime MadeContact { get; set; } = DateTime.Now;
        public int CurrentLocation { get; set; }
        public Flight()
        {
            
           
            
        }


       

        public override string ToString()
        {
            var status = IsArrival ? "Arriving" : "Departing";
            return $"flight number: {FlightNumber}, status:{status}, registered at airport at:{MadeContact:G}";
        }
    }
}
