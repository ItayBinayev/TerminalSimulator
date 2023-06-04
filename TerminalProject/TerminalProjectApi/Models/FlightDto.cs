using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalProjectApi.Models
{
    public class FlightDto
    {
        public int FlightNumber { get; set; }

        public bool IsArrival { get; set; }

        public string? AirLine { get; set; }

        public DateTime MadeContactAt { get; set; }

        private static readonly string[] _airlines = { "Delta Air", "American Airlines", "Lufthansa", "Air France", "Southwest", "Emirates", "British Airways", "El Al", "EasyJet", "AirAsia" };



        public FlightDto()
        {
            Random random = new();
            FlightNumber = random.Next(1000000);
            random = new();
            IsArrival = random.Next(0, 2) == 0;
            
            MadeContactAt = DateTime.Now;
            AirLine = _airlines[random.Next(_airlines.Length)];
        }

        public override string ToString()
        {
            var status = IsArrival ? "Departing" : "Arriving";
            return $"flight number: {FlightNumber}, status:{status}, registered at airport at:{MadeContactAt:G}";
        }
    }
}
