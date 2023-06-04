using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NLog;
using NLog.Fluent;
using TerminalProjectApi.Database;
using TerminalProjectApi.Hubs;
using TerminalProjectApi.Interfaces;
using LogLevel = NLog.LogLevel;

namespace TerminalProjectApi.Models
{
    public class TestControlTower : IControlTower
    {
        private Flight leg4;
        private Flight leg6;
        private Flight leg7;

        private readonly DataDbContext _dbContext;
        private static readonly Logger logger = LogManager.GetLogger("FlightLogger");
        private readonly IHubContext<TerminalHub> _terminalHubContext;


        public List<Flight> Leg1 { get; set; } = new List<Flight>();
        public List<Flight> Leg2 { get; set; } = new List<Flight>();
        public List<Flight> Leg3 { get; set; } = new List<Flight>();
        public Flight Leg4
        {
            get { return leg4; }
            set
            {
                {

                    leg4 = value;
                    if (leg4 == null)
                    {
                        OnLeg4BecameNullEvent();
                    }

                }

                ;
            }
        }
        public List<Flight> Leg5 { get; set; } = new List<Flight>();
        public Flight Leg6
        {
            get { return leg6; }
            set
            {
                {

                    leg6 = value;
                    if (leg6 == null)
                    {
                        OnLeg6BecameNullEvent();
                    }

                }

                ;
            }
        }
        public Flight Leg7
        {
            get { return leg7; }
            set
            {
                {

                    leg7 = value;
                    if (leg7 == null)
                    {
                        OnLeg7BecameNullEvent();
                    }

                }

                ;
            }
        }
        public List<Flight> Leg8 { get; set; } = new List<Flight>();
        public List<Flight> Leg9 { get; set; } = new List<Flight>();
        public Queue<Flight> Waitlist4 { get; set; } = new Queue<Flight>();
        public Queue<Flight> Waitlist67 { get; set; } = new Queue<Flight>();

        public event EventHandler Leg4BecameNullEvent;
        public event EventHandler Leg6BecameNullEvent;
        public event EventHandler Leg7BecameNullEvent;

        public TestControlTower(DataDbContext dbContext, IHubContext<TerminalHub> flightHubContext)
        {
            _dbContext = dbContext;
            _terminalHubContext = flightHubContext;
            Leg4 = null;
            Leg6 = null;
            Leg7 = null;
            Leg4BecameNullEvent += Leg4BecameNullHandler;
            Leg6BecameNullEvent += Leg6BecameNullHandler;
            Leg7BecameNullEvent += Leg7BecameNullHandler;
        }

        public async Task DeleteFlight(Flight flight)
        {

            if (flight == null)
            {
                Console.WriteLine("There isnt flight with the exist ID");
                return;
            }

            _dbContext.Flights.Remove(flight);
            _dbContext.SaveChangesAsync();
            _terminalHubContext.Clients.All.SendAsync("FlightDismissed", flight.Id);
            var hisF = new HistoryFlight {FlightNumber = flight.FlightNumber, IsArrival = flight.IsArrival, AirLine = flight.AirLine,MadeContact = flight.MadeContact};

            var logEvent = new LogEventInfo(LogLevel.Info, logger.Name, "Flight Landed / Taken Off : ");
            logEvent.Properties["Object"] = hisF;
            logger.Log(logEvent);
            Console.WriteLine("Delete Succeded");
        }


        public async Task UpdateFlight(Flight flight)
        {
            var existingFlight = await _dbContext.Flights.FindAsync(flight.Id);

            if (existingFlight == null)
            {
                Console.WriteLine("Not Exist Flight!");
            }

            // Update the properties of the existingFlight object with the new values
            existingFlight.CurrentLocation = flight.CurrentLocation;
            // ...

            _dbContext.SaveChangesAsync(); // Save the changes to the database
            _terminalHubContext.Clients.All.SendAsync("LegChanged", flight, flight.CurrentLocation);

            Console.WriteLine("Flight " + flight.FlightNumber +" Been Updated!");
        }

        public async Task MoveFlight(Flight f)
        {
            System.Timers.Timer timer = new System.Timers.Timer(6000);
            timer.Elapsed += async (o, s) =>
            {
                switch (f.CurrentLocation)
                {
                    case 0:
                        Console.WriteLine("Code : " + f.FlightNumber + " - Current Location : " + f.CurrentLocation);
                        if (f.IsArrival)
                        {
                            Leg1.Add(f);
                            f.CurrentLocation = 1;
                             await UpdateFlight(f);
                            break;
                        }
                        else
                        {
                            if (!Waitlist67.Contains(f))
                            {
                                Waitlist67.Enqueue(f);
                            }
                            if (Leg6 == null)
                            {
                                
                                Leg6 = null;

                            }
                            else if (Leg7 == null)
                            {
                                Leg7 = null;
                                
                            }
                           

                            break;
                        }
                    case 1:
                        {
                            Console.WriteLine("Code : " + f.FlightNumber + " - Current Location : " + f.CurrentLocation);
                            Leg1.Remove(f);
                            Leg2.Add(f);
                            f.CurrentLocation = 2;
                            await UpdateFlight(f);

                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Code : " + f.FlightNumber + " - Current Location : " + f.CurrentLocation);
                            Leg2.Remove(f);
                            Leg3.Add(f);
                            f.CurrentLocation = 3;
                            await UpdateFlight(f);

                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("Code : " + f.FlightNumber + " - Current Location : " + f.CurrentLocation);

                            if (!Waitlist4.Contains(f))
                            {
                                Waitlist4.Enqueue(f);
                            }
                            if (Leg4 == null)
                            {
                                Leg3.Remove(f);
                                Leg4 = null;
                                
                            }


                            break;
                        }
                    case 4:
                        {
                            Console.WriteLine("Code : " + f.FlightNumber + " - Current Location : " + f.CurrentLocation);
                            if (f.IsArrival)
                            {
                                Leg5.Add(f);
                                f.CurrentLocation = 5;
                                await UpdateFlight(f);
                                Leg4 = null;

                                break;
                            }
                            else
                            {
                                Leg9.Add(f);
                                f.CurrentLocation = 9;
                                await UpdateFlight(f);
                                Leg4 = null;

                                break;
                            }
                        }
                    case 5:
                        Console.WriteLine("Code : " + f.FlightNumber + " - Current Location : " + f.CurrentLocation);
                        if (!Waitlist67.Contains(f))
                        {
                            Waitlist67.Enqueue(f);
                        }
                        if (Leg6 == null)
                        {
                            Leg5.Remove(f);
                            Leg6 = null;
                            

                        }
                        else if (Leg7 == null)
                        {
                            Leg5.Remove(f);
                            Leg7 = null;
                            
                        }
                        

                        break;
                    case 6:
                        {
                            Console.WriteLine("Code : " + f.FlightNumber + " - Current Location : " + f.CurrentLocation);
                            if (f.IsArrival)
                            {
                                Leg6 = null;
                                //Add To The Flights History
                                //Removing From The Database
                                //Console.WriteLine("Code : " + f.FlightNumber + " - Current Location : " + f.CurrentLocation);
                                await DeleteFlight(f);
                                timer.Stop();
                                //_dbContext.Flights.Remove(f);
                                //await _dbContext.SaveChangesAsync();



                                break;
                            }
                            else
                            {
                                Leg6 = null;
                                Leg8.Add(f);
                                //Waitlist4.Enqueue(f);
                                f.CurrentLocation = 8;
                                await UpdateFlight(f);
                                break;
                            }
                        }
                    case 7:
                        {
                            Console.WriteLine("Code : " + f.FlightNumber + " - Current Location : " + f.CurrentLocation);
                            if (f.IsArrival)
                            {
                                Leg7 = null;
                                //Add To The Flights History
                                //Removing From The Database
                                await DeleteFlight(f);
                                timer.Stop();
                                //_dbContext.Flights.Remove(f);
                                //await _dbContext.SaveChangesAsync();

                                //need to add to the log history
                                break;
                            }
                            else
                            {
                                Leg7 = null;
                                Leg8.Add(f);
                                //Waitlist4.Enqueue(f);
                                f.CurrentLocation = 8;
                                await UpdateFlight(f);
                                break;
                            }
                        }
                    case 8:
                        {
                            Console.WriteLine("Code : " + f.FlightNumber + " - Current Location : " + f.CurrentLocation);
                            if (!Waitlist4.Contains(f))
                            {
                                Waitlist4.Enqueue(f);
                            }
                            if (Leg4 == null)
                            {
                                Leg8.Remove(f);
                                Leg4 = null;
                                //Leg4[0] = f;
                                //Leg4[0].CurrentLocation = 4;
                            }
                            break;
                        }
                    case 9:
                        {
                            Console.WriteLine("Code : " + f.FlightNumber + " - Current Location : " + f.CurrentLocation);
                            Leg9.Remove(f);
                            //Add To The Flights History
                            //Removing From The Database
                            await DeleteFlight(f);
                            timer.Stop();
                            //_dbContext.Flights.Remove(f);
                            //await _dbContext.SaveChangesAsync();

                            break;
                        }
                }
           };
            timer.Start();
        } //Event per flight
        protected void OnLeg4BecameNullEvent()
        {
            Leg4BecameNullEvent?.Invoke(null, EventArgs.Empty);
        }
        protected void OnLeg6BecameNullEvent()
        {
            Leg6BecameNullEvent?.Invoke(null, EventArgs.Empty);
        }
        protected void OnLeg7BecameNullEvent()
        {
            Leg7BecameNullEvent?.Invoke(null, EventArgs.Empty);
        }
        private void Leg4BecameNullHandler(object sender, EventArgs e)
        {
            if (Waitlist4.Count != 0)
            {
                Leg4 = Waitlist4.Dequeue();
                if (Leg4.CurrentLocation == 8)
                {
                    Leg8.Remove(Leg4);
                }
                else if (Leg4.CurrentLocation == 3)
                {
                    Leg3.Remove(Leg4);
                }
                Leg4.CurrentLocation = 4;
                Console.WriteLine("Code : " + Leg4.FlightNumber + " - Current Location : " + Leg4.CurrentLocation + " (In Event)");
                UpdateFlight(Leg4);

            }
        }
        private void Leg6BecameNullHandler(object sender, EventArgs e)
        {

            if (Waitlist67.Count != 0)
            {
                Leg6 = Waitlist67.Dequeue();
                if (Leg6.CurrentLocation == 5)
                {
                    Leg5.Remove(Leg6);
                }
                Leg6.CurrentLocation = 6;
                Console.WriteLine("Code : " + Leg6.FlightNumber + " - Current Location : " + Leg6.CurrentLocation + " (In Event)");
                UpdateFlight(Leg6);
            }
        }
        private void Leg7BecameNullHandler(object sender, EventArgs e)
        {
            if (Waitlist67.Count != 0)
            {
                Leg7 = Waitlist67.Dequeue();
                if (Leg7.CurrentLocation == 5)
                {
                    Leg5.Remove(Leg6);
                }
                Leg7.CurrentLocation = 7;
                Console.WriteLine("Code : " + Leg7.FlightNumber + " - Current Location : " + Leg7.CurrentLocation + " (In Event)");
                UpdateFlight(Leg7);
            }
        }
    }
}