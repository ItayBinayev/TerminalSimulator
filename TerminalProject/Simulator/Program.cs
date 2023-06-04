using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using TerminalProjectApi.Models;

namespace Simulator
{
    internal class Program
    {
        static readonly HttpClient client = new() { BaseAddress = new Uri("http://localhost:5192") };
        static void Main(string[] args)
        {
            var timer = new System.Timers.Timer(15000);
            timer.Elapsed += (s, e) => CreateFlight();
            timer.Elapsed += (s, e) => ChangeTimerInterval(s!);
            timer.Start();
            Console.WriteLine("simulator started");
            int counter = 0;
            Console.ReadKey();
        }

        private static async void CreateFlight()
        {
            var flight = new FlightDto();
            Console.WriteLine($"Flight sent:");
            //Console.WriteLine("Before changing id");
            Console.WriteLine($"{flight}");
            //Console.WriteLine("ID " + flight.Id);
            try
            {
                var response = await client.PostAsJsonAsync("api/Flights", flight);
                //var jsonResponse = await response.Content.ReadAsStringAsync();
                //var idObject = JObject.Parse(jsonResponse);
                //var id = idObject["id"].Value<int>();
                //flight.Id = id;

                //Console.WriteLine("Respnse:");
                //Console.WriteLine(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void ChangeTimerInterval(object source)
        {
            var timer = source as System.Timers.Timer;
            Random rnd = new Random();
            timer!.Interval = rnd.Next(15000, 15200);
        }


    }
}
